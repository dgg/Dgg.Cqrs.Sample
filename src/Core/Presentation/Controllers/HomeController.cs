using System.Web.Mvc;

namespace Dgg.Cqrs.Sample.Core.Presentation.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Message = "Welcome to CQRS bug tracker!";
			ViewBag.Message = 

			return View();
		}

		public ActionResult About()
		{
			return View();
		}
	}
}
