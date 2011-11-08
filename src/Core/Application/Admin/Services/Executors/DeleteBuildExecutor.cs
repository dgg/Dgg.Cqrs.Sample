using System;
using Dgg.Cqrs.Sample.Core.Application.Admin.Commands;
using Dgg.Cqrs.Sample.Core.Domain.Admin.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Services.Executors
{
	public class DeleteBuildExecutor : CommandExecutor<DeleteBuild>
	{
		private readonly IValidationService _validation;
		private readonly ISnapshotSession _snapshots;
		private readonly IEventSession _events;
		private readonly IDocumentSession _models;
		private readonly EventBus<BuildDeleted> _bus;

		public DeleteBuildExecutor(IValidationService validation, ISnapshotSession snapshots, IEventSession events, IDocumentSession models, EventBus<BuildDeleted> bus)
		{
			_validation = validation;
			_snapshots = snapshots;
			_events = events;
			_models = models;
			_bus = bus;
		}

		public override void Execute(DeleteBuild cmd)
		{
			_validation.AssertValidity(cmd);

			try
			{
				var e = new BuildDeleted(cmd.Id);
				_bus.Publish(e);

				_models.SaveChanges();
			}
			catch (Exception)
			{
				_events.RollbackChanges();
				_snapshots.RollbackChanges();
				throw;
			}
		}
	}
}
