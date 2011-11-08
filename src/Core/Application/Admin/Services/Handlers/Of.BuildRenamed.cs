using Dgg.Cqrs.Sample.Core.Domain.Admin.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Services.Handlers
{
	public class BuildRenamedPersister : IHandler<BuildRenamed>
	{
		private readonly IEventSession _eventSession;

		public BuildRenamedPersister(IEventSession eventSession)
		{
			_eventSession = eventSession;
		}

		public bool Handle(BuildRenamed e)
		{
			_eventSession.Save(e);
			return true;
		}
	}

	public class BuildRenamedQueryUpdater : IHandler<BuildRenamed>
	{
		private readonly IDocumentSession _modelSession;

		public BuildRenamedQueryUpdater(IDocumentSession modelSession)
		{
			_modelSession = modelSession;
		}

		public bool Handle(BuildRenamed e)
		{
			var model = _modelSession.Single<Presentation.Models.Admin.Build>(e.ReceiverId);
			model.Name = e.NewName;

			return true;
		}
	}
}
