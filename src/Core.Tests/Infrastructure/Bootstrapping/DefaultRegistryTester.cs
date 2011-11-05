using Dgg.Cqrs.Sample.Core.Application.Admin.Services;
using Dgg.Cqrs.Sample.Core.Infrastructure.Bootstrapping;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation;
using NUnit.Framework;
using Raven.Client;
using StructureMap;

namespace Dgg.Cqrs.Sample.Core.Tests.Infrastructure.Bootstrapping
{
	[TestFixture]
	public class DefaultRegistryTester
	{
		[Test]
		public void Install_TypesWithStandardConvention_AreRegistered()
		{
			IContainer container = new Container(new DefaultRegistry());

			Assert.That(container.Model.DefaultTypeFor<IApplicationService>(), Is.EqualTo(typeof(ApplicationService)));
			Assert.That(container.Model.DefaultTypeFor<IValidationService>(), Is.EqualTo(typeof(ValidationService)));
			Assert.That(container.Model.DefaultTypeFor<ISnapshotSession>(), Is.EqualTo(typeof(SnapshotSession)));
			Assert.That(container.Model.DefaultTypeFor<ISnapshotSessionFactory>(), Is.EqualTo(typeof(SnapshotSessionFactory)));
			Assert.That(container.Model.DefaultTypeFor<ISnapshotSessionFactory>(), Is.EqualTo(typeof(SnapshotSessionFactory)));
			Assert.That(container.Model.DefaultTypeFor<IQuerySessionFactoryBuilder>(), Is.EqualTo(typeof(QuerySessionFactoryBuilder)));
		}

		[Test]
		public void Install_Validation_CanBeResolved()
		{
			IContainer container = new Container(new DefaultRegistry());

			Assert.That(container.GetInstance<IValidationService>(), Is.InstanceOf<ValidationService>());
			
		}
	}
}
