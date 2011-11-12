using System;
using Dgg.Cqrs.Sample.Core.Application.DefectHandling.Commands;
using Dgg.Cqrs.Sample.Core.Domain.DefectHandling.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.DefectHandling.Services.Executors
{
	public class DeleteIssueExecutor : CommandExecutor<DeleteIssue>
	{
		private readonly IValidationService _validation;
		private readonly ISnapshotSession _snapshots;
		private readonly EventBus<IssueDeleted> _bus;
		private readonly IDocumentSession _modelSession;

		public DeleteIssueExecutor(IValidationService validation, ISnapshotSession snapshots, EventBus<IssueDeleted> bus, IDocumentSession modelSession)
		{
			_validation = validation;
			_snapshots = snapshots;
			_bus = bus;
			_modelSession = modelSession;
		}

		public override void Execute(DeleteIssue cmd)
		{
			_validation.AssertValidity(cmd);

			try
			{
				var e = new IssueDeleted(cmd.Id);
				_bus.Publish(e);

				_modelSession.SaveChanges();
			}
			catch (Exception)
			{
				_snapshots.RollbackChanges();
				throw;
			}
		}
	}
}