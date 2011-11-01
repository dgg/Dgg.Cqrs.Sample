using System;
using Raven.Client.Document;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Data
{
	public class QuerySessionFactoryBuilder : IQuerySessionFactoryBuilder
	{
		private readonly Lazy<IQuerySessionFactory> _sessionFactory = new Lazy<IQuerySessionFactory>(initSessionFactory);

		public IQuerySessionFactory GetSessionFactory()
		{
			return _sessionFactory.Value;
		}

		private static IQuerySessionFactory initSessionFactory()
		{
			DocumentStore store = new DocumentStore { ConnectionStringName = "Queries" };
			return new QuerySessionFactory(store);
		}
	}
}
