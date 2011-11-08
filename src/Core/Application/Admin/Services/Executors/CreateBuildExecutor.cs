using System;
using Dgg.Cqrs.Sample.Core.Application.Admin.Commands;
using Dgg.Cqrs.Sample.Core.Domain.Admin;
using Dgg.Cqrs.Sample.Core.Domain.Admin.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Services.Executors
{
	public class CreateBuildExecutor : CommandExecutor<CreateBuild>
	{
		private readonly IValidationService _validation;
		private readonly ISnapshotSession _snapshots;
		private readonly IEventSession _events;
		private readonly IDocumentSession _models;
		private readonly EventBus<BuildCreated> _bus;

		public CreateBuildExecutor(IValidationService validation, ISnapshotSession snapshots, IEventSession events, IDocumentSession models, EventBus<BuildCreated> bus)
		{
			_validation = validation;
			_snapshots = snapshots;
			_events = events;
			_models = models;
			_bus = bus;
		}

		public override void Execute(CreateBuild cmd)
		{
			_validation.AssertValidity(cmd);

			DomainEventHandler<BuildCreated> callBack = (sender, e) =>
			{
				_bus.Subscribe(e.EntityHandler);
				_bus.Publish(e.Event);
			};
			try
			{
				Build.Creating += callBack;
				Build build = new Build(cmd.Name);

				_snapshots.Save(build);
				_models.SaveChanges();
			}
			catch (Exception)
			{
				_events.RollbackChanges();
				_snapshots.RollbackChanges();
				throw;
			}
			finally
			{
				Build.Creating -= callBack;
			}
		}
	}
}