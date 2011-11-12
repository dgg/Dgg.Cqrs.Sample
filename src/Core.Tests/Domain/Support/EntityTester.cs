using System;
using Dgg.Cqrs.Sample.Core.Infrastructure;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using NUnit.Framework;

namespace Dgg.Cqrs.Sample.Core.Tests.Domain.Support
{
	[TestFixture]
	public abstract class EntityTester
	{
		protected void EventApplier<TEvent>(object sender, DomainEventEventArgs<TEvent> e) where TEvent : DomainEvent
		{
			e.EntityHandler.Handle(e.Event);
		}

		protected void HappeningIn(DateTimeOffset when, Action whatHappened)
		{
			Time.UtcProvider = () => when;
			whatHappened();
			Time.UtcProvider = null;
		}
	}
}
