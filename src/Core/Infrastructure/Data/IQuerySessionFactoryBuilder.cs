namespace Dgg.Cqrs.Sample.Core.Infrastructure.Data
{
	public interface IQuerySessionFactoryBuilder
	{
		IQuerySessionFactory GetSessionFactory();
	}
}
