using StructureMap.Configuration.DSL;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Bootstrapping
{
	public class DefaultRegistry : Registry
	{
		public DefaultRegistry()
		{
			Scan(scanner =>
			{
				scanner.AssemblyContainingType<Application.Admin.Services.IApplicationService>();
				scanner.WithDefaultConventions();
			});
		}
	}
}