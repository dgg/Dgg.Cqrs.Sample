using Raven.Client.Document;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Data
{
	public class QuerySessionFactoryBuilder : IQuerySessionFactoryBuilder
	{
		private IQuerySessionFactory _sessionFactory;

		public IQuerySessionFactory GetSessionFactory()
		{
			return _sessionFactory ?? (_sessionFactory = initSessionFactory());
		}

		private static IQuerySessionFactory initSessionFactory()
		{
			DocumentStore store = new DocumentStore { ConnectionStringName = "Queries" };
			return new QuerySessionFactory(store);
		}
	}
}
