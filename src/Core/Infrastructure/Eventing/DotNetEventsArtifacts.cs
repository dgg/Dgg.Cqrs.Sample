using System;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Eventing
{
	public class DomainEventEventArgs<TEvent> : EventArgs where TEvent : DomainEvent
	{
		public static readonly Func<TEvent, bool> NoOp = _ => true;

		/// <summary>
		/// used when the event does not requires an handler in the receiver
		/// </summary>
		public DomainEventEventArgs(TEvent e) : this(e, NoOp) {  }

		/// <summary>
		/// used when the event requires a handlers in the receiver
		/// </summary>
		/// <param name="e"></param>
		/// <param name="handler"></param>
		public DomainEventEventArgs(TEvent e, Func<TEvent, bool> handler)
		{
			Event = e;
			EntityHandler = new GenericHandler<TEvent>(handler);
		}

		public TEvent Event { get; private set; }

		public IHandler<TEvent> EntityHandler { get; private set; }
	}

	public delegate void DomainEventHandler<TEvent>(object sender, DomainEventEventArgs<TEvent> e) where TEvent : DomainEvent;
}