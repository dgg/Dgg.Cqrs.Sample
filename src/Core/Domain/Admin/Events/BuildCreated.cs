using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;

namespace Dgg.Cqrs.Sample.Core.Domain.Admin.Events
{
	public class BuildCreated : DomainEvent
	{
		public BuildCreated(Guid receiverId) : base(receiverId) { }
		public BuildCreated(Guid receiverId, Guid eventId, DateTimeOffset timeStamp) : base(receiverId, eventId, timeStamp) { }

		public string Name { get; set; }
	}
}
