using Dgg.Cqrs.Sample.Core.Domain.DefectHandling;
using Dgg.Cqrs.Sample.Core.Domain.DefectHandling.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using Dgg.Cqrs.Sample.Core.Presentation.Models.DefectHandling;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.DefectHandling.Services.Handlers
{
	public class IssueClosedPersister : IHandler<IssueClosed>
	{
		private readonly IEventSession _eventSession;

		public IssueClosedPersister(IEventSession eventSession)
		{
			_eventSession = eventSession;
		}

		public bool Handle(IssueClosed e)
		{
			_eventSession.Save(e);
			return true;
		}
	}

	/// <summary>
	/// As we are dealing with object databases, we have to delete the old object in the old state
	/// </summary>
	public class IssueClosedSnapshotUpdater : IHandler<IssueClosed>
	{
		private readonly ISnapshotSession _snapshotSession;

		public IssueClosedSnapshotUpdater(ISnapshotSession snapshotSession)
		{
			_snapshotSession = snapshotSession;
		}

		public bool Handle(IssueClosed e)
		{
			_snapshotSession.Delete((FixedIssue @fixed) => @fixed.Id.Equals(e.ReceiverId));
			return true;
		}
	}

	public class IssueClosedQueryUpdater : IHandler<IssueClosed>
	{
		private readonly IDocumentSession _modelSession;

		public IssueClosedQueryUpdater(IDocumentSession modelSession)
		{
			_modelSession = modelSession;
		}

		public bool Handle(IssueClosed e)
		{
			Presentation.Models.DefectHandling.Issue @fixed =
				_modelSession.Single<Presentation.Models.DefectHandling.Issue>(e.ReceiverId);

			@fixed.Closed = e.Closed;
			@fixed.Status= IssueStatus.Closed;

			return true;
		}
	}
}