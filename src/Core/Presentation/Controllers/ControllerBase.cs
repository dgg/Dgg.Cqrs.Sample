using System;
using System.Web.Mvc;

namespace Dgg.Cqrs.Sample.Core.Presentation.Controllers
{
	public abstract class ControllerBase : Controller
	{
		public ActionResult Validating<TCommand>(TCommand model, Func<ActionResult> doWhenValid) where TCommand : Infrastructure.Commanding.ICommand
		{
			return ModelState.IsValid ? doWhenValid() : View(model);
		}

		public ActionResult Validating<TCommand>(TCommand model, Func<ActionResult> doWhenValid, Action<TCommand> doBeforeRedirecting) where TCommand : Infrastructure.Commanding.ICommand
		{
			if (ModelState.IsValid) return doWhenValid();

			doBeforeRedirecting(model);
			return View(model);
		}

		public ActionResult Validating<TCommand, TModel>(TCommand model, Func<ActionResult> doWhenValid, Func<TCommand, TModel> doBeforeRedirecting) where TCommand : Infrastructure.Commanding.ICommand
		{
			if (ModelState.IsValid) return doWhenValid();

			return View(doBeforeRedirecting(model));
		}
	}
}
