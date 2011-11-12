using System;
using Dgg.Cqrs.Sample.Core.Domain.DefectHandling;
using Dgg.Cqrs.Sample.Core.Domain.DefectHandling.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation;
using Raven.Client;
using OpenIssue = Dgg.Cqrs.Sample.Core.Application.DefectHandling.Commands.OpenIssue;

namespace Dgg.Cqrs.Sample.Core.Application.DefectHandling.Services.Executors
{
	public class OpenIssueExecutor : CommandExecutor<OpenIssue>
	{
		private readonly IValidationService _validation;
		private readonly ISnapshotSession _snapshots;
		private readonly EventBus<IssueOpened> _bus;
		private readonly IDocumentSession _modelSession;

		public OpenIssueExecutor(IValidationService validation, ISnapshotSession snapshots, EventBus<IssueOpened> bus, IDocumentSession modelSession)
		{
			_validation = validation;
			_snapshots = snapshots;
			_bus = bus;
			_modelSession = modelSession;
		}

		public override void Execute(OpenIssue cmd)
		{
			_validation.AssertValidity(cmd);

			DomainEventHandler<IssueOpened> callBack = (sender, e) =>
			{
				_bus.Subscribe(e.EntityHandler);
				_bus.Publish(e.Event);
			};
			try
			{
				Domain.DefectHandling.OpenIssue.Opening += callBack;
				var solution = _snapshots.Single<Solution>(s => s.Id == cmd.SolutionId);
				var version = _snapshots.Single<AppVersion>(s => s.Id == cmd.VersionId);
				var build = _snapshots.Single<Build>(s => s.Id == cmd.BuildId);
				Domain.DefectHandling.OpenIssue issue = Issue.Open(solution, version, build, cmd.Title, cmd.Description, cmd.StepsToReproduce, cmd.ExpectedResult, cmd.ActualResult);

				_snapshots.Save(issue);
				_modelSession.SaveChanges();
			}
			catch (Exception)
			{
				_snapshots.RollbackChanges();
				throw;
			}
			finally
			{
				Domain.DefectHandling.OpenIssue.Opening -= callBack;
			}
		}
	}
}
