using Dgg.Cqrs.Sample.Core.Domain.DefectHandling;
using Dgg.Cqrs.Sample.Core.Domain.DefectHandling.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.DefectHandling.Services.Handlers
{
	public class IssueDeletedPersister : IHandler<IssueDeleted>
	{
		private readonly IEventSession _eventSession;

		public IssueDeletedPersister(IEventSession eventSession)
		{
			_eventSession = eventSession;
		}

		public bool Handle(IssueDeleted e)
		{
			_eventSession.Save(e);
			return true;
		}
	}

	public class IssueDeletedSnapshotUpdater : IHandler<IssueDeleted>
	{
		private readonly ISnapshotSession _snapshotSession;

		public IssueDeletedSnapshotUpdater(ISnapshotSession snapshotSession)
		{
			_snapshotSession = snapshotSession;
		}

		public bool Handle(IssueDeleted e)
		{
			_snapshotSession.Delete<Issue>(s => s.Id == e.ReceiverId);

			return true;
		}
	}

	public class IssueDeletedQueryUpdater : IHandler<IssueDeleted>
	{
		private readonly IDocumentSession _modelSession;

		public IssueDeletedQueryUpdater(IDocumentSession modelSession)
		{
			_modelSession = modelSession;
		}

		public bool Handle(IssueDeleted e)
		{
			var toBeDeleted = _modelSession.Single<Presentation.Models.DefectHandling.Issue>(e.ReceiverId);
			_modelSession.Delete(toBeDeleted);
			return true;
		}
	}
}
