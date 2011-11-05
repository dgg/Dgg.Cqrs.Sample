using System.Web;
using StructureMap.Configuration.DSL;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Bootstrapping
{
	public class SystemWebRegistry : Registry
	{
		public SystemWebRegistry()
		{
			For<HttpContextBase>().Use(() => new HttpContextWrapper(HttpContext.Current));
		}
	}
}
