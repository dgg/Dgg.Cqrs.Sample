using Dgg.Cqrs.Sample.Core.Domain.DefectHandling;
using Dgg.Cqrs.Sample.Core.Domain.DefectHandling.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using Dgg.Cqrs.Sample.Core.Presentation.Models.DefectHandling;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.DefectHandling.Services.Handlers
{
	public class IssueFixedPersister : IHandler<IssueFixed>
	{
		private readonly IEventSession _eventSession;

		public IssueFixedPersister(IEventSession eventSession)
		{
			_eventSession = eventSession;
		}

		public bool Handle(IssueFixed e)
		{
			_eventSession.Save(e);
			return true;
		}
	}

	/// <summary>
	/// As we are dealing with object databases, we have to delete the old object in the old state
	/// </summary>
	public class IssueFixedSnapshotUpdater : IHandler<IssueFixed>
	{
		private readonly ISnapshotSession _snapshotSession;

		public IssueFixedSnapshotUpdater(ISnapshotSession snapshotSession)
		{
			_snapshotSession = snapshotSession;
		}

		public bool Handle(IssueFixed e)
		{
			_snapshotSession.Delete((OpenIssue open) => open.Id.Equals(e.ReceiverId));
			return true;
		}
	}

	public class IssueFixedQueryUpdater : IHandler<IssueFixed>
	{
		private readonly IDocumentSession _modelSession;

		public IssueFixedQueryUpdater(IDocumentSession modelSession)
		{
			_modelSession = modelSession;
		}

		public bool Handle(IssueFixed e)
		{
			Presentation.Models.DefectHandling.Issue open =
				_modelSession.Single<Presentation.Models.DefectHandling.Issue>(e.ReceiverId);

			open.VersionFixed = new Presentation.Models.DefectHandling.AppVersion
			{
				Id = e.VersionFixed.Id.ToString(),
				Name = e.VersionFixed.Name
			};

			open.BuildFixed = new Presentation.Models.DefectHandling.Build
			{
				Id = e.BuildFixed.Id.ToString(),
				Name = e.BuildFixed.Name
			};

			open.Fixed = e.Fixed;
			open.Status= IssueStatus.Fixed;
			return true;
		}
	}
}