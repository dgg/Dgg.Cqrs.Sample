using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;

namespace Dgg.Cqrs.Sample.Core.Domain.DefectHandling.Events
{
	public class IssueClosed : DomainEvent
	{
		public IssueClosed(Guid receiverId) : base(receiverId) { }

		public IssueClosed(Guid receiverId, Guid eventId, DateTimeOffset timeStamp) : base(receiverId, eventId, timeStamp) { }

		public DateTimeOffset Closed { get; set; }
	}
}