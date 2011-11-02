using System.Web.Mvc;

namespace Dgg.Cqrs.Sample.Core.Presentation.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			return View();
		}
	}
}
