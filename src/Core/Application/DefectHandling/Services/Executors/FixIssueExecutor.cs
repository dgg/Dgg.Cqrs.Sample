using System;
using Dgg.Anug.Cqrs.Core.Domain.DefectHandling.Events;
using Dgg.Cqrs.Sample.Core.Application.DefectHandling.Commands;
using Dgg.Cqrs.Sample.Core.Domain.DefectHandling;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation;
using Raven.Client;
using OpenIssue = Dgg.Cqrs.Sample.Core.Domain.DefectHandling.OpenIssue;

namespace Dgg.Cqrs.Sample.Core.Application.DefectHandling.Services.Executors
{
	public class FixIssueExecutor : CommandExecutor<FixIssue>
	{
		private readonly IValidationService _validation;
		private readonly ISnapshotSession _snapshots;
		private readonly EventBus<IssueFixed> _bus;
		private readonly IDocumentSession _modelSession;

		public FixIssueExecutor(IValidationService validation, ISnapshotSession snapshots, EventBus<IssueFixed> bus, IDocumentSession modelSession)
		{
			_validation = validation;
			_snapshots = snapshots;
			_bus = bus;
			_modelSession = modelSession;
		}

		public override void Execute(FixIssue cmd)
		{
			_validation.AssertValidity(cmd);

			DomainEventHandler<IssueFixed> callBack = (sender, e) =>
			{
				_bus.Subscribe(e.EntityHandler);
				_bus.Publish(e.Event);
			};
			try
			{
				FixedIssue.Fixing += callBack;
				
				var fixedInVersion = _snapshots.Single<AppVersion>(s => s.Id == cmd.FixedInVersionId);
				var fixedInBuild = _snapshots.Single<Build>(s => s.Id == cmd.FixedInBuildId);

				OpenIssue toFix = _snapshots.Single<OpenIssue>(i => i.Id.Equals(cmd.Id));
				FixedIssue @fixed = toFix.Fix(fixedInVersion, fixedInBuild, cmd.Resolution);

				_snapshots.Save(@fixed);
				_modelSession.SaveChanges();
			}
			catch (Exception)
			{
				_snapshots.RollbackChanges();
				throw;
			}
			finally
			{
				FixedIssue.Fixing -= callBack;
			}
		}
	}
}