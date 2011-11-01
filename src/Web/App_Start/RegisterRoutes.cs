using System.Web.Mvc;
using System.Web.Routing;
using Dgg.Cqrs.Sample.Core.Infrastructure.Bootstrapping;

[assembly: WebActivator.PostApplicationStartMethod(typeof(Dgg.Cqrs.Sample.Web.App_Start.Bootstrapper), "PostStart")]

namespace Dgg.Cqrs.Sample.Web.App_Start
{
	public static class Bootstrapper
	{
		public static void PostStart()
		{
			new ApplicationBootstrapper()
				.RegisterAreas()
				.RegisterRoutes(RouteTable.Routes)
				.RegisterGlobalFilters(GlobalFilters.Filters)
				.ConfigureIoC();
		}
	}
}