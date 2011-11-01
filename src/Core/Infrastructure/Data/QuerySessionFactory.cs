using Raven.Client;
using Raven.Client.Indexes;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Data
{
	internal class QuerySessionFactory : IQuerySessionFactory
	{
		private readonly IDocumentStore _store;

		public QuerySessionFactory(IDocumentStore store)
		{
			if (_store == null)
			{
				_store = store;
				_store.Initialize();
				IndexCreation.CreateIndexes(GetType().Assembly, _store);
			}
		}

		public IDocumentSession CreateSession()
		{
			return _store.OpenSession();
		}
	}
}