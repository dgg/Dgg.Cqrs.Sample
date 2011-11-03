using System;
using System.Web.Mvc;

namespace Dgg.Cqrs.Sample.Core.Presentation.Controllers
{
	public abstract class ControllerBase : Controller
	{
		public ActionResult Validating<T>(T model, Func<ActionResult> doWhenValid)
		{
			return ModelState.IsValid ? doWhenValid() : View(model);
		}

		public ActionResult Validating<T>(T model, Func<ActionResult> doWhenValid, Action<T> doBeforeRedirecting)
		{
			if (ModelState.IsValid) return doWhenValid();

			doBeforeRedirecting(model);
			return View(model);
		}
	}
}
