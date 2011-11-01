using Dgg.Cqrs.Sample.Core.Presentation.Controllers;
using StructureMap.Configuration.DSL;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Bootstrapping
{
	public class ControllerRegistry : Registry
	{
		public ControllerRegistry()
		{
			Scan(scanner =>
			{
				scanner.TheCallingAssembly();
				scanner.IncludeNamespaceContainingType<HomeController>();
				scanner.With(new ControllerConvention());
			});
		}
	}
}