namespace Dgg.Cqrs.Sample.Core.Infrastructure.Eventing
{
	public interface IHandler<in TEvent> where TEvent : DomainEvent
	{
		bool Handle(TEvent e);
	}
}
