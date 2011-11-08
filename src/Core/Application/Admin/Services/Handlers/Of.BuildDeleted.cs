using Dgg.Cqrs.Sample.Core.Domain.Admin;
using Dgg.Cqrs.Sample.Core.Domain.Admin.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Services.Handlers
{
	public class BuildDeletedPersister : IHandler<BuildDeleted>
	{
		private readonly IEventSession _eventSession;

		public BuildDeletedPersister(IEventSession eventSession)
		{
			_eventSession = eventSession;
		}

		public bool Handle(BuildDeleted e)
		{
			_eventSession.Save(e);
			return true;
		}
	}

	public class BuildDeletedSnapshotUpdater : IHandler<BuildDeleted>
	{
		private readonly ISnapshotSession _snapshotSession;

		public BuildDeletedSnapshotUpdater(ISnapshotSession snapshotSession)
		{
			_snapshotSession = snapshotSession;
		}

		public bool Handle(BuildDeleted e)
		{
			_snapshotSession.Delete<Build>(s => s.Id == e.ReceiverId);

			return true;
		}
	}

	public class BuildDeletedQueryUpdater : IHandler<BuildDeleted>
	{
		private readonly IDocumentSession _modelSession;

		public BuildDeletedQueryUpdater(IDocumentSession modelSession)
		{
			_modelSession = modelSession;
		}

		public bool Handle(BuildDeleted e)
		{
			var toBeDeleted = _modelSession.Single<Presentation.Models.Admin.Build>(e.ReceiverId);
			_modelSession.Delete(toBeDeleted);
			return true;
		}
	}
}
