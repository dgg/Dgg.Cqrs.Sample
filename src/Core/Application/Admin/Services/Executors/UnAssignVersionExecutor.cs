using System;
using Dgg.Cqrs.Sample.Core.Application.Admin.Commands;
using Dgg.Cqrs.Sample.Core.Domain.Admin;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Services.Executors
{
	public class UnAssignVersionExecutor : CommandExecutor<UnAssignVersion>
	{
		private readonly IValidationService _validator;
		private readonly ISnapshotSession _snapshots;
		private readonly IDocumentSession _models;

		public UnAssignVersionExecutor(IValidationService validator, ISnapshotSession snapshots, IDocumentSession models)
		{
			_validator = validator;
			_snapshots = snapshots;
			_models = models;
		}

		public override void Execute(UnAssignVersion cmd)
		{
			_validator.AssertValidity(cmd);

			try
			{
				AppVersion unAssigned = unAssignSnapshot(cmd);
				updateModel(unAssigned);

				_models.SaveChanges();
			}
			catch (Exception)
			{
				_snapshots.RollbackChanges();
				throw;
			}
		}

		private AppVersion unAssignSnapshot(UnAssignVersion cmd)
		{
			AppVersion version = _snapshots.Single<AppVersion>(v => v.Id == cmd.Id);
			version.Assign(null);
			_snapshots.Save(version);
			return version;
		}

		private void updateModel(AppVersion e)
		{
			var model = _models.Single<Presentation.Models.Admin.AppVersion>(e.Id);
			model.AssignedTo = null;
		}
	}
}
