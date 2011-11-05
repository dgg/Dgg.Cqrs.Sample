using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Dgg.Cqrs.Sample.Core.Infrastructure.Bootstrapping;
using StructureMap;

namespace Dgg.Cqrs.Sample.Web
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			// does not use WebActivator as we still need to wire to the EndRequest
			new ApplicationBootstrapper()
				.RegisterAreas()
				.RegisterRoutes(RouteTable.Routes)
				.RegisterGlobalFilters(GlobalFilters.Filters)
				.ConfigureIoC(ObjectFactory.Container);
		}

		protected void Application_EndRequest()
		{
			new ApplicationTeardown().WhenEndingRequest();
		}
	}
}