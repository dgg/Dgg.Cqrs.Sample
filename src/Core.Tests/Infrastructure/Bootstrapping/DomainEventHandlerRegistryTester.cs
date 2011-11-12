using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Bootstrapping;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using NUnit.Framework;
using StructureMap;

namespace Dgg.Cqrs.Sample.Core.Tests.Infrastructure.Bootstrapping
{
	[TestFixture]
	public class DomainEventHandlerRegistryTester
	{
		[Test]
		public void RegistersInstances_IHandlerOfEvent1()
		{
			IContainer container = new Container(new DomainEventHandlersRegistry(typeof(DomainEventHandlerRegistryTester)));
			
			Assert.That(container.GetAllInstances(typeof(IHandler<Event1>)), Has.Count.EqualTo(2).And.
				Some.InstanceOf<Handler1_1>().And.
				Some.InstanceOf<Handler1_2>());
		}

		[Test]
		public void RegistersInstances_IHandlerOfEvent2()
		{
			IContainer container = new Container(new DomainEventHandlersRegistry(typeof(DomainEventHandlerRegistryTester)));

			Assert.That(container.GetAllInstances(typeof(IHandler<Event2>)), Has.Count.EqualTo(1).And.
				Some.InstanceOf<Handler2_1>());
		}

		[Test]
		public void AllInstances_CanBeInjected()
		{
			IContainer container = new Container(new DomainEventHandlersRegistry(typeof(DomainEventHandlerRegistryTester)));
			container.Configure(config => config.For<UsesHandlersOfEvent1>());

			var instance = container.GetInstance<UsesHandlersOfEvent1>();
			Assert.That(instance.Handlers, Has.Length.EqualTo(2).And.
				Some.InstanceOf<Handler1_1>().And.
				Some.InstanceOf<Handler1_2>());
		}
	}

	public class Event1 : DomainEvent
	{
		public Event1(Guid receiverId) : base(receiverId) { }

		public Event1(Guid receiverId, Guid eventId, DateTimeOffset timeStamp) : base(receiverId, eventId, timeStamp) { }
	}

	public class Event2 : DomainEvent
	{
		public Event2(Guid receiverId) : base(receiverId) { }

		public Event2(Guid receiverId, Guid eventId, DateTimeOffset timeStamp) : base(receiverId, eventId, timeStamp) { }
	}

	public class Handler1_1 : IHandler<Event1>
	{
		public bool Handle(Event1 e) { return true; }
	}

	public class Handler1_2 : IHandler<Event1>
	{
		public bool Handle(Event1 e) { return true; }
	}

	public class Handler2_1 : IHandler<Event2>
	{
		public bool Handle(Event2 e) { return true; }
	}

	public class UsesHandlersOfEvent1
	{
		public UsesHandlersOfEvent1(IHandler<Event1>[] handlers)
		{
			Handlers = handlers;
		}

		internal IHandler<Event1>[] Handlers { get; set;}
	}
}
