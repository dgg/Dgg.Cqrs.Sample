using System;
using Dgg.Cqrs.Sample.Core.Application.DefectHandling.Commands;
using Dgg.Cqrs.Sample.Core.Domain.DefectHandling;
using Dgg.Cqrs.Sample.Core.Domain.DefectHandling.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.DefectHandling.Services.Executors
{
	public class CloseIssueExecutor : CommandExecutor<CloseIssue>
	{
		private readonly IValidationService _validation;
		private readonly ISnapshotSession _snapshots;
		private readonly EventBus<IssueClosed> _bus;
		private readonly IDocumentSession _modelSession;

		public CloseIssueExecutor(IValidationService validation, ISnapshotSession snapshots, EventBus<IssueClosed> bus, IDocumentSession modelSession)
		{
			_validation = validation;
			_snapshots = snapshots;
			_bus = bus;
			_modelSession = modelSession;
		}

		public override void Execute(CloseIssue cmd)
		{
			_validation.AssertValidity(cmd);

			DomainEventHandler<IssueClosed> callBack = (sender, e) =>
			{
				_bus.Subscribe(e.EntityHandler);
				_bus.Publish(e.Event);
			};
			try
			{
				ClosedIssue.Closing += callBack;
				
				FixedIssue toClose = _snapshots.Single<FixedIssue>(i => i.Id.Equals(cmd.Id));

				ClosedIssue closed = toClose.Close();

				_snapshots.Save(closed);
				_modelSession.SaveChanges();
			}
			catch (Exception)
			{
				_snapshots.RollbackChanges();
				throw;
			}
			finally
			{
				ClosedIssue.Closing -= callBack;
			}
		}
	}
}