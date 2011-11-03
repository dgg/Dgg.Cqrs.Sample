using System;
using System.Collections.Generic;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Eventing
{
	public class EventBus<TEvent> where TEvent : DomainEvent
	{
		private readonly List<IHandler<TEvent>> _handlers;

		public EventBus(IHandler<TEvent>[] handlers)
		{
			_handlers = new List<IHandler<TEvent>>(handlers.Length);
			Array.ForEach(handlers, h =>
			{
				if (h != null) _handlers.Add(h);
			});
		}

		public void Publish(TEvent e)
		{
			_handlers.ForEach(h => h.Handle(e));
		}

		public EventBus<TEvent> Subscribe(IHandler<TEvent> handler)
		{
			if (handler != null) _handlers.Add(handler);
			return this;
		}
	}
}
