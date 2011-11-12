using System;
using System.Linq;
using Dgg.Cqrs.Sample.Core.Domain.Admin.Events;
using Dgg.Cqrs.Sample.Core.Domain.DefectHandling.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.DefectHandling.Services.Handlers
{
	public class BuildDeletedSnapshotUpdater : IHandler<BuildDeleted>
	{
		private readonly ISnapshotSession _snapshotSession;

		public BuildDeletedSnapshotUpdater(ISnapshotSession snapshotSession)
		{
			_snapshotSession = snapshotSession;
		}

		public bool Handle(BuildDeleted e)
		{
			_snapshotSession.Delete<Domain.DefectHandling.Build>(s => s.Id.Equals(e.ReceiverId));
			return true;
		}
	}

	public class BuildDeletedIssueSnapshotUpdater : IHandler<BuildDeleted>
	{
		private readonly ISnapshotSession _snapshotSession;
		private readonly EventBus<IssueDeleted> _bus;

		public BuildDeletedIssueSnapshotUpdater(ISnapshotSession snapshotSession, EventBus<IssueDeleted> bus)
		{
			_snapshotSession = snapshotSession;
			_bus = bus;
		}

		public bool Handle(BuildDeleted e)
		{
			_snapshotSession.All<Domain.DefectHandling.Issue>()
				.Where(i => i.Build.Id == e.ReceiverId)
				.ToList()
				.ForEach(i => _bus.Publish(new IssueDeleted(i.Id)));
			return true;
		}
	}

	public class BuildDeletedEntitiesQueryUpdater : IHandler<BuildDeleted>
	{
		private readonly IDocumentSession _modelSession;

		public BuildDeletedEntitiesQueryUpdater(IDocumentSession modelSession)
		{
			_modelSession = modelSession;
		}

		public bool Handle(BuildDeleted e)
		{
			var entities = _modelSession.Single<Presentation.Models.DefectHandling.SupportEntities>(
				Presentation.Models.DefectHandling.SupportEntities.TheId);
			var toBeDeleted = entities.Builds.Single(s => s.Id.Equals(e.ReceiverId.ToString(), StringComparison.Ordinal));
			entities.Builds.Remove(toBeDeleted);

			return true;
		}
	}
}
