using System.Web.Mvc;
using System.Web.Routing;
using Dgg.Cqrs.Sample.Core.Infrastructure.Web;
using StructureMap;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Bootstrapping
{
	public class ApplicationBootstrapper
	{
		public ApplicationBootstrapper RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("favicon.ico");

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);

			return this;
		}

		public ApplicationBootstrapper ConfigureIoC()
		{
			ObjectFactory.Configure(configuration =>
				configuration.Scan(scanner =>
				{
					scanner.TheCallingAssembly();
					scanner.LookForRegistries();
				}));
			DependencyResolver.SetResolver(new SmDependencyResolver(ObjectFactory.Container));
			return this;
		}

		public ApplicationBootstrapper RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
			return this;
		}

		public ApplicationBootstrapper RegisterAreas()
		{
			AreaRegistration.RegisterAllAreas();
			return this;
		}
	}
}