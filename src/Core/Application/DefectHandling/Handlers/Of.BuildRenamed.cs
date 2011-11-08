using System;
using System.Linq;
using Dgg.Cqrs.Sample.Core.Application.DefectHandling.Queries;
using Dgg.Cqrs.Sample.Core.Domain.Admin.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.DefectHandling.Handlers
{
	public class BuildRenamedSnapshotUpdater : IHandler<BuildRenamed>
	{
		private readonly ISnapshotSession _snapshotSession;

		public BuildRenamedSnapshotUpdater(ISnapshotSession snapshotSession)
		{
			_snapshotSession = snapshotSession;
		}

		public bool Handle(BuildRenamed e)
		{
			var build = _snapshotSession.Single<Domain.DefectHandling.Build>(s => s.Id.Equals(e.ReceiverId));
			build.Name = e.NewName;
			_snapshotSession.Save(build);
			return true;
		}
	}

	public class BuildRenamedEntitiesQueryUpdater : IHandler<BuildRenamed>
	{
		private readonly IDocumentSession _modelSession;

		public BuildRenamedEntitiesQueryUpdater(IDocumentSession modelSession)
		{
			_modelSession = modelSession;
		}

		public bool Handle(BuildRenamed e)
		{
			var entities = _modelSession.Single<Presentation.Models.DefectHandling.SupportEntities>(
				Presentation.Models.DefectHandling.SupportEntities.TheId);
			var toBeRenamed = entities.Builds.Single(s => s.Id.Equals(e.ReceiverId.ToString(), StringComparison.Ordinal));
			toBeRenamed.Name = e.NewName;

			return true;
		}
	}

	public class BuildRenamedIssuesQueryUpdater : IHandler<BuildRenamed>
	{
		private readonly IDocumentSession _modelSession;

		public BuildRenamedIssuesQueryUpdater(IDocumentSession modelSession)
		{
			_modelSession = modelSession;
		}

		public bool Handle(BuildRenamed e)
		{
			var issues = _modelSession.Query<Presentation.Models.DefectHandling.Issue, IssuesAssignedToBuild>()
				.Where(i => i.Build.Id == e.ReceiverId.ToString());
			foreach (var issue in issues)
			{
				issue.Build.Name = e.NewName;
			}

			return true;
		}
	}
}
