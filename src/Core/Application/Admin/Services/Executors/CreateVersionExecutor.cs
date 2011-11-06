using System;
using System.Collections.Generic;
using System.Linq;
using Dgg.Cqrs.Sample.Core.Application.Admin.Commands;
using Dgg.Cqrs.Sample.Core.Domain.Admin;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Services.Executors
{
	public class CreateVersionExecutor : CommandExecutor<CreateVersion>
	{
		private readonly IValidationService _validation;
		private readonly ISnapshotSession _snapshots;
		private readonly IDocumentSession _models;

		public CreateVersionExecutor(IValidationService validation, ISnapshotSession snapshots, IDocumentSession models)
		{
			_validation = validation;
			_snapshots = snapshots;
			_models = models;
		}

		public override void Execute(CreateVersion cmd)
		{
			_validation.AssertValidity(cmd);

			try
			{

				AppVersion version = createSnapshot(cmd);
				updateModelForCreation(version);

				createHandlingSnapshot(version);
				updateHandlingModel(version);

				_models.SaveChanges();
			}
			catch (Exception)
			{
				_snapshots.RollbackChanges();
				throw;
			}
		}

		private AppVersion createSnapshot(CreateVersion command)
		{
			AppVersion version = new AppVersion(command.Name);
			_snapshots.Save(version);
			return version;
		}

		private void updateModelForCreation(AppVersion e)
		{
			_models.Store(new Presentation.Models.Admin.AppVersion { Id = e.Id.ToString(), Name = e.Name });
		}

		private void createHandlingSnapshot(AppVersion e)
		{
			Domain.DefectHandling.AppVersion ds = new Domain.DefectHandling.AppVersion(e.Id, e.Name);
			_snapshots.Save(ds);
		}

		private void updateHandlingModel(AppVersion e)
		{
			if (!documentExists()) createSupportEntitiesFor(e);
			else updateSupportEntitiesFor(e);
		}

		private bool documentExists()
		{
			return _models.Query<Presentation.Models.DefectHandling.SupportEntities>().Any();
		}

		private void createSupportEntitiesFor(AppVersion e)
		{
			_models.Store(Presentation.Models.DefectHandling.SupportEntities.FirstWith(
				new Presentation.Models.DefectHandling.AppVersion { Id = e.Id.ToString(), Name = e.Name }));
		}

		private void updateSupportEntitiesFor(AppVersion e)
		{
			// is not optimal to get the whole document for adding to an internal array, but...
			var entities = _models.Single<Presentation.Models.DefectHandling.SupportEntities>(
				Presentation.Models.DefectHandling.SupportEntities.TheId);
			var versions = entities.Versions ?? new List<Presentation.Models.DefectHandling.AppVersion>();
			versions.Add(new Presentation.Models.DefectHandling.AppVersion
			{
				Id = e.Id.ToString(),
				Name = e.Name
			});
			entities.Versions = versions;
		}
	}
}
