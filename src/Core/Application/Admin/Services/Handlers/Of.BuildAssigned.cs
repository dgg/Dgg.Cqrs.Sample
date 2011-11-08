using Dgg.Cqrs.Sample.Core.Domain.Admin.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Services.Handlers
{
	public class BuildAssignedPersister : IHandler<BuildAssigned>
	{
		private readonly IEventSession _eventSession;

		public BuildAssignedPersister(IEventSession eventSession)
		{
			_eventSession = eventSession;
		}

		public bool Handle(BuildAssigned e)
		{
			_eventSession.Save(e);
			return true;
		}
	}

	public class BuildAssignedQueryUpdater : IHandler<BuildAssigned>
	{
		private readonly IDocumentSession _modelSession;

		public BuildAssignedQueryUpdater(IDocumentSession modelSession)
		{
			_modelSession = modelSession;
		}

		public bool Handle(BuildAssigned e)
		{
			var model = _modelSession.Single<Presentation.Models.Admin.Build>(e.ReceiverId);
			var solution = e.NewlyAssigned;
			model.AssignedTo = solution != null ?
				new Presentation.Models.Admin.Solution { Id = e.NewlyAssigned.Id.ToString(), Name = e.NewlyAssigned.Name } :
				null;

			return true;
		}
	}
}
