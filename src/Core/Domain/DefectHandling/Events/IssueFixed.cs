using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;

namespace Dgg.Cqrs.Sample.Core.Domain.DefectHandling.Events
{
	public class IssueFixed : DomainEvent
	{
		public IssueFixed(Guid receiverId) : base(receiverId) { }

		public IssueFixed(Guid receiverId, Guid eventId, DateTimeOffset timeStamp) : base(receiverId, eventId, timeStamp) { }

		public AppVersion VersionFixed { get; set; }
		public Build BuildFixed { get; set; }
		public string Resolution { get; set; }

		public DateTimeOffset Fixed { get; set; }
	}
}
