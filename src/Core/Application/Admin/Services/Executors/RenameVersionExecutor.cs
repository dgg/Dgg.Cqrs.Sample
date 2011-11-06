using System;
using System.Linq;
using Dgg.Cqrs.Sample.Core.Application.Admin.Commands;
using Dgg.Cqrs.Sample.Core.Domain.Admin;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Services.Executors
{
	public class RenameVersionExecutor : CommandExecutor<RenameVersion>
	{
		private readonly IValidationService _validation;
		private readonly ISnapshotSession _snapshots;
		private readonly IDocumentSession _models;

		public RenameVersionExecutor(IValidationService validation, ISnapshotSession snapshots, IDocumentSession models)
		{
			_validation = validation;
			_snapshots = snapshots;
			_models = models;
		}

		public override void Execute(RenameVersion cmd)
		{
			_validation.AssertValidity(cmd);

			try
			{
				AppVersion renamed = renameSnapshot(cmd);
				renameModel(renamed);

				renameHandlingSnapshot(renamed);
				renameSupportEntities(renamed);
				renameHandlingAssignedModels(renamed);

				_models.SaveChanges();
			}
			catch (Exception)
			{
				_snapshots.RollbackChanges();
				throw;
			}
		}

		private AppVersion renameSnapshot(RenameVersion cmd)
		{
			var version = _snapshots.Single<AppVersion>(s => s.Id == cmd.Id);
			version.Rename(cmd.NewName);

			_snapshots.Save(version);

			return version;
		}

		private void renameModel(AppVersion e)
		{
			var model = _models.Single<Presentation.Models.Admin.AppVersion>(e.Id);
			model.Name = e.Name;	
		}

		private void renameHandlingSnapshot(AppVersion e)
		{
			var version = _snapshots.Single<Domain.DefectHandling.AppVersion>(s => s.Id.Equals(e.Id));
			version.Name = e.Name;
			_snapshots.Save(version);
		}

		private void renameSupportEntities(AppVersion e)
		{
			var entities = _models.Single<Presentation.Models.DefectHandling.SupportEntities>(
				Presentation.Models.DefectHandling.SupportEntities.TheId);
			var toBeRenamed = entities.Versions.Single(s => s.Id.Equals(e.Id.ToString(), StringComparison.Ordinal));
			toBeRenamed.Name = e.Name;
		}

		private void renameHandlingAssignedModels(AppVersion e)
		{
			var issues = _models.Query<Presentation.Models.DefectHandling.Issue, DefectHandling.Queries.IssuesAssignedToVersion>()
				.Where(i => i.Version.Id == e.Id.ToString());

			foreach (var issue in issues)
			{
				issue.Version.Name = e.Name;
			}
		}
	}
}
