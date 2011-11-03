using System;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Eventing
{
	public class GenericHandler<TEvent> : IHandler<TEvent> where TEvent : DomainEvent
	{
		private readonly Func<TEvent, bool> _handler;

		public GenericHandler(Func<TEvent, bool> handler)
		{
			_handler = handler;
		}

		public bool Handle(TEvent e)
		{
			return _handler(e);
		}
	}
}
