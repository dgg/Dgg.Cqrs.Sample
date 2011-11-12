using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;

namespace Dgg.Cqrs.Sample.Core.Domain.DefectHandling.Events
{
	public class IssueOpened : DomainEvent
	{
		public IssueOpened(Guid receiverId) : base(receiverId) { }

		public IssueOpened(Guid receiverId, Guid eventId, DateTimeOffset timeStamp) : base(receiverId, eventId, timeStamp) { }

		public Solution Solution { get; set; }
		public AppVersion Version { get; set; }
		public Build Build { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }

		public string StepsToReproduce { get; set; }
		public string ExpectedResult { get; set; }
		public string ActualResult { get; set; }

		public DateTimeOffset Opened { get; set; }
	}
}
