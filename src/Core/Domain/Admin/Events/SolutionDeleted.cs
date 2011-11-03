using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;

namespace Dgg.Cqrs.Sample.Core.Domain.Admin.Events
{
	public class SolutionDeleted : DomainEvent
	{
		public SolutionDeleted(Guid receiverId) : base(receiverId) { }

		// TODO: delete if not used
		[Obsolete("would it ever be used?")]
		public SolutionDeleted(Guid receiverId, Guid eventId, DateTimeOffset timeStamp) : base(receiverId, eventId, timeStamp) { }
	}
}
