using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;

namespace Dgg.Cqrs.Sample.Core.Domain.Admin.Events
{
	public class BuildAssigned : DomainEvent
	{
		public BuildAssigned(Guid receiverId) : base(receiverId) { }
		public BuildAssigned(Guid receiverId, Guid eventId, DateTimeOffset timeStamp) : base(receiverId, eventId, timeStamp) { }

		public Solution PreviouslyAssigned { get; set; }
		public Solution NewlyAssigned { get; set; }
	}
}