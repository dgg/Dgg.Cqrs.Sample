using System.Collections.Generic;
using Dgg.Cqrs.Sample.Core.Domain.Admin.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.DefectHandling.Services.Handlers
{
	public class BuildCreatedSnapshotCreator : IHandler<BuildCreated>
	{
		private readonly ISnapshotSession _snapshotSession;

		public BuildCreatedSnapshotCreator(ISnapshotSession snapshotSession)
		{
			_snapshotSession = snapshotSession;
		}

		public bool Handle(BuildCreated e)
		{
			Domain.DefectHandling.Build ds = new Domain.DefectHandling.Build(e.ReceiverId, e.Name);
			_snapshotSession.Save(ds);
			return true;
		}
	}

	public class BuildCreatedEntitiesQueryUpdater : IHandler<BuildCreated>
	{
		private readonly IDocumentSession _modelSession;

		public BuildCreatedEntitiesQueryUpdater(IDocumentSession modelSession)
		{
			_modelSession = modelSession;
		}

		public bool Handle(BuildCreated e)
		{
			if (!documentExists())
			{
				createSupportEntitiesFor(e);
			}
			else
			{
				updateSupportEntitiesFor(e);
			}
			return true;
		}

		private bool documentExists()
		{
			// since we have a document not commited to the db, we use load instead of query, again, inneficient
			return _modelSession.Single<Presentation.Models.DefectHandling.SupportEntities>(Presentation.Models.DefectHandling.SupportEntities.TheId) != null;
		}

		private void createSupportEntitiesFor(BuildCreated e)
		{
			_modelSession.Store(Presentation.Models.DefectHandling.SupportEntities.FirstWith(
				new Presentation.Models.DefectHandling.Build { Id = e.ReceiverId.ToString(), Name = e.Name }));
		}

		private void updateSupportEntitiesFor(BuildCreated e)
		{
			// is not optimal to get the whole document for adding to an internal array, but...
			var entities = _modelSession.Single<Presentation.Models.DefectHandling.SupportEntities>(
				Presentation.Models.DefectHandling.SupportEntities.TheId);
			var builds = entities.Builds ?? new List<Presentation.Models.DefectHandling.Build>();
			builds.Add(new Presentation.Models.DefectHandling.Build
			{
				Id = e.ReceiverId.ToString(),
				Name = e.Name
			});
			entities.Builds = builds;
		}
	}
}
