using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;

namespace Dgg.Cqrs.Sample.Core.Domain.Admin.Events
{
	public class BuildRenamed : DomainEvent
	{
		public BuildRenamed(Guid receiverId) : base(receiverId) { }
		public BuildRenamed(Guid receiverId, Guid eventId, DateTimeOffset timeStamp) : base(receiverId, eventId, timeStamp) { }

		public string OldName { get; set; }
		public string NewName { get; set; }
	}
}