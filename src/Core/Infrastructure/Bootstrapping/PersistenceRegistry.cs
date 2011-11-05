using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Raven.Client;
using StructureMap.Configuration.DSL;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Bootstrapping
{
	public class PersistenceRegistry : Registry
	{
		public PersistenceRegistry()
		{
			For<IQuerySessionFactoryBuilder>().Singleton().Use<QuerySessionFactoryBuilder>();
			For<IDocumentSession>()
				.HybridHttpOrThreadLocalScoped()	
				.AddInstances(inst => inst.ConstructedBy(context =>
					context.GetInstance<IQuerySessionFactoryBuilder>().GetSessionFactory().CreateSession()));

			/*For<IEventSession>()
				.HybridHttpOrThreadLocalScoped()
				.Use(context => context.GetInstance<IEventSessionFactory>().CreateSession());*/

			For<ISnapshotSession>()
				.HybridHttpOrThreadLocalScoped()
				.Use(context => context.GetInstance<ISnapshotSessionFactory>().CreateSession());
		}
	}
}
