using System;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Eventing
{
	public abstract class DomainEvent
	{
		protected DomainEvent(Guid receiverId) : this(receiverId, Comb.Generate(), Time.UtcNow) { }

		protected DomainEvent(Guid receiverId, Guid eventId, DateTimeOffset timeStamp)
		{
			ReceiverId = receiverId;
			EventId = eventId;
			TimeStamp = timeStamp;
		}

		public Guid ReceiverId { get; set; }
		public Guid EventId { get; private set; }
		public DateTimeOffset TimeStamp { get; private set; }
	}
}
