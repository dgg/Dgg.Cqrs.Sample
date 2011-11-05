using Dgg.Cqrs.Sample.Core.Infrastructure.Bootstrapping;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using NUnit.Framework;
using Raven.Client;
using StructureMap;

namespace Dgg.Cqrs.Sample.Core.Tests.Infrastructure.Bootstrapping
{
	[TestFixture]
	public class PersistenceRegistryTester
	{
		[Test]
		public void Install_QueryFactoryBuilder_IsSingleton()
		{
			IContainer container = new Container(new PersistenceRegistry());

			Assert.That(container.Model.For<IQuerySessionFactoryBuilder>().Lifecycle, Is.EqualTo("Singleton"));
		}

		[Test]
		public void Install_IDocumentStore_IsRegistered()
		{
			IContainer container = new Container(new PersistenceRegistry());

			Assert.That(container.Model.HasDefaultImplementationFor<IDocumentSession>(), Is.True);
		}
	}
}
