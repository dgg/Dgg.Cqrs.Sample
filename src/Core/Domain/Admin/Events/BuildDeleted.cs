using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;

namespace Dgg.Cqrs.Sample.Core.Domain.Admin.Events
{
	public class BuildDeleted : DomainEvent
	{
		public BuildDeleted(Guid receiverId) : base(receiverId) { }
	}
}
