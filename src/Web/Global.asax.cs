using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Dgg.Cqrs.Sample.Core.Infrastructure.Bootstrapping;

namespace Dgg.Cqrs.Sample.Web
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			new ApplicationBootstrapper()
				.RegisterAreas()
				.RegisterRoutes(RouteTable.Routes)
				.RegisterGlobalFilters(GlobalFilters.Filters)
				.ConfigureIoC();
		}
	}
}