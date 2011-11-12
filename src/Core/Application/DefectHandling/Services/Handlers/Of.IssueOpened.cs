using Dgg.Cqrs.Sample.Core.Domain.DefectHandling.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.DefectHandling.Services.Handlers
{
	public class IssueOpenedPersister : IHandler<IssueOpened>
	{
		private readonly IEventSession _eventSession;

		public IssueOpenedPersister(IEventSession eventSession)
		{
			_eventSession = eventSession;
		}

		public bool Handle(IssueOpened e)
		{
			_eventSession.Save(e);
			return true;
		}
	}

	public class IssueOpenedQueryUpdater : IHandler<IssueOpened>
	{
		private readonly IDocumentSession _modelSession;

		public IssueOpenedQueryUpdater(IDocumentSession modelSession)
		{
			_modelSession = modelSession;
		}

		public bool Handle(IssueOpened e)
		{
			var solution = new Presentation.Models.DefectHandling.Solution
			{
				Id = e.Solution.Id.ToString(),
				Name = e.Solution.Name
			};

			var version = new Presentation.Models.DefectHandling.AppVersion
			{
				Id = e.Version.Id.ToString(),
				Name = e.Version.Name
			};

			var build = new Presentation.Models.DefectHandling.Build
			{
				Id = e.Build.Id.ToString(),
				Name = e.Build.Name
			};

			_modelSession.Store(new Presentation.Models.DefectHandling.Issue
			{
				Id = e.ReceiverId.ToString(),
				Title = e.Title,
				Solution = solution,
				Version = version,
				Build = build,
				Description = e.Description,
				StepsToReproduce = e.Description,
				ExpectedResult = e.ExpectedResult,
				ActualResult = e.ActualResult,
				Opened = e.Opened
			});

			return true;
		}
	}
}