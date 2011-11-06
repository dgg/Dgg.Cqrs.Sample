namespace Dgg.Cqrs.Sample.Core.Infrastructure.Data
{
	public interface IEventSessionFactory
	{
		IEventSession CreateSession();
	}
}