using System.Web;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Data
{
	public class SnapshotSessionFactory : Db4oSessionFactory, ISnapshotSessionFactory
	{
		public SnapshotSessionFactory(HttpContextBase ctx) : base(ctx) { }

		public ISnapshotSession CreateSession()
		{
			return new SnapshotSession(GetContainer("Snapshots"));
		}
	}
}
