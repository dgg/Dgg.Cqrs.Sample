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
	public class DeleteVersionExecutor : CommandExecutor<DeleteVersion>
	{
		private readonly IValidationService _validation;
		private readonly ISnapshotSession _snapshots;
		private readonly IDocumentSession _models;

		public DeleteVersionExecutor(IValidationService validation, ISnapshotSession snapshots, IDocumentSession models)
		{
			_validation = validation;
			_snapshots = snapshots;
			_models = models;
		}

		public override void Execute(DeleteVersion cmd)
		{
			_validation.AssertValidity(cmd);

			try
			{
				deleteSnapshot(cmd);
				deleteModel(cmd);

				deleteHandlingSnapshot(cmd);
				deleteHandlingModel(cmd);
				deleteHandlingAssignedModels(cmd);

				_models.SaveChanges();
			}
			catch (Exception)
			{
				_snapshots.RollbackChanges();
				throw;
			}
		}

		private void deleteSnapshot(DeleteVersion cmd)
		{
			_snapshots.Delete<AppVersion>(s => s.Id == cmd.Id);
		}

		private void deleteModel(DeleteVersion cmd)
		{
			var toBeDeleted = _models.Single<Presentation.Models.Admin.AppVersion>(cmd.Id);
			_models.Delete(toBeDeleted);
		}

		private void deleteHandlingSnapshot(DeleteVersion cmd)
		{
			_snapshots.Delete<Domain.DefectHandling.AppVersion>(s => s.Id.Equals(cmd.Id));
		}

		private void deleteHandlingModel(DeleteVersion cmd)
		{
			var entities = _models.Single<Presentation.Models.DefectHandling.SupportEntities>(
				Presentation.Models.DefectHandling.SupportEntities.TheId);
			var toBeDeleted = entities.Versions.Single(s => s.Id.Equals(cmd.Id.ToString(), StringComparison.Ordinal));
			entities.Versions.Remove(toBeDeleted);
		}

		private void deleteHandlingAssignedModels(DeleteVersion cmd)
		{
			Presentation.Models.DefectHandling.Issue[] relatedIssues = _models.Query<Presentation.Models.DefectHandling.Issue>()
				.Where(i => i.Version.Id == cmd.Id.ToString())
				.ToArray();

			Array.ForEach(relatedIssues, i => _models.Delete(i));
		}
	}
}
