using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;

namespace Dgg.Cqrs.Sample.Core.Domain.Admin.Events
{
	public class SolutionCreated : DomainEvent
	{
		public SolutionCreated(Guid receiverId) : base(receiverId) { }

		public string Name { get; set; }
	}
}
