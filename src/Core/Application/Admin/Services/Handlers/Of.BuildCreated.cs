using Dgg.Cqrs.Sample.Core.Domain.Admin.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Services.Handlers
{
	public class BuildCreatedPersister : IHandler<BuildCreated>
	{
		private readonly IEventSession _eventSession;

		public BuildCreatedPersister(IEventSession eventSession)
		{
			_eventSession = eventSession;
		}

		public bool Handle(BuildCreated e)
		{
			_eventSession.Save(e);
			return true;
		}
	}

	public class BuildCreatedQueryUpdater : IHandler<BuildCreated>
	{
		private readonly IDocumentSession _modelSession;

		public BuildCreatedQueryUpdater(IDocumentSession modelSession)
		{
			_modelSession = modelSession;
		}

		public bool Handle(BuildCreated e)
		{
			_modelSession.Store(new Presentation.Models.Admin.Build { Id = e.ReceiverId.ToString(), Name = e.Name });
			return true;
		}
	}
}
