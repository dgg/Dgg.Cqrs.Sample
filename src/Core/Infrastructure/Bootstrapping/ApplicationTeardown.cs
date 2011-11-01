using StructureMap;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Bootstrapping
{
	public class ApplicationTeardown
	{
		public ApplicationTeardown WhenEndingRequest()
		{
			ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
			return this;
		}
	}
}
