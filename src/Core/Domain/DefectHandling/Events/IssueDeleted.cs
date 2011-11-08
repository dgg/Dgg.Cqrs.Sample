using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;

namespace Dgg.Cqrs.Sample.Core.Domain.DefectHandling.Events
{
	public class IssueDeleted : DomainEvent
	{
		public IssueDeleted(Guid receiverId) : base(receiverId) { }
	}
}
