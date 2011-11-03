using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;

namespace Dgg.Cqrs.Sample.Core.Domain.Admin.Events
{
	public class SolutionRenamed : DomainEvent
	{
		public SolutionRenamed(Guid receiverId) : base(receiverId) { }

		// TODO: delete if not used
		[Obsolete("would it ever be used?")]
		public SolutionRenamed(Guid receiverId, Guid eventId, DateTimeOffset timeStamp) : base(receiverId, eventId, timeStamp) { }

		public string OldName { get; set; }
		public string NewName { get; set; }
	}
}
