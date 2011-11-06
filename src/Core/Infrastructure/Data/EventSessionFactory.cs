using System.Web;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Data
{
	public class EventSessionFactory : Db4oSessionFactory, IEventSessionFactory
	{
		public EventSessionFactory(HttpContextBase ctx): base(ctx){ }

		public IEventSession CreateSession()
		{
			return new EventSession(GetContainer("Events"));
		}
	}
}
