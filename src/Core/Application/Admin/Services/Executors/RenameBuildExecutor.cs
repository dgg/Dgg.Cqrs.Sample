using System;
using Dgg.Cqrs.Sample.Core.Application.Admin.Commands;
using Dgg.Cqrs.Sample.Core.Domain.Admin;
using Dgg.Cqrs.Sample.Core.Domain.Admin.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Services.Executors
{
	public class RenameBuildExecutor : Infrastructure.Commanding.CommandExecutor<RenameBuild>
	{
		private readonly IValidationService _validation;
		private readonly ISnapshotSession _snapshots;
		private readonly IEventSession _events;
		private readonly IDocumentSession _models;
		private readonly EventBus<BuildRenamed> _bus;

		public RenameBuildExecutor(IValidationService validation, ISnapshotSession snapshots, IEventSession events, IDocumentSession models, EventBus<BuildRenamed> bus)
		{
			_validation = validation;
			_snapshots = snapshots;
			_events = events;
			_models = models;
			_bus = bus;
		}

		public override void Execute(RenameBuild cmd)
		{
			_validation.AssertValidity(cmd);

			try
			{
				Build subject = _snapshots.Single<Build>(s => s.Id == cmd.Id);
				subject.Renaming += (sender, e) =>
				{
					_bus.Subscribe(e.EntityHandler);
					_bus.Publish(e.Event);
				};
				subject.Rename(cmd.NewName);

				_snapshots.Save(subject);
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
