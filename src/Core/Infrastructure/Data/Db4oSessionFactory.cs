using System.Configuration;
using System.IO;
using System.Web;
using Db4objects.Db4o;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Data
{
	public abstract class Db4oSessionFactory
	{
		private readonly HttpContextBase _ctx;

		protected Db4oSessionFactory(HttpContextBase ctx)
		{
			_ctx = ctx;
		}

		protected IObjectContainer GetContainer(string connectionStringName)
		{
			string _dbPath = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
			if (_dbPath.Contains("|DataDirectory|"))
			{
				//we know, then, that this is a web project
				//and HttpContext is hopefully not null...
				_dbPath = _dbPath.Replace("|DataDirectory|", "");
				string appDir = _ctx.Server.MapPath("~/App_Data/");
				_dbPath = Path.Combine(appDir, _dbPath);
			}
			IObjectContainer container = Db4oFactory.OpenFile(_dbPath);
			return container;
		}
	}
}