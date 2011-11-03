using System;
using System.Web.Mvc;
using Dgg.Cqrs.Sample.Core.Application.Admin.Commands;
using Dgg.Cqrs.Sample.Core.Application.Admin.Queries;
using Dgg.Cqrs.Sample.Core.Application.Admin.Services;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Presentation.Models.Admin;

namespace Dgg.Cqrs.Sample.Core.Presentation.Controllers
{
	public class SolutionController : ControllerBase
	{
		private readonly IQueryRepository _queries;
		private readonly IApplicationService _service;
		private readonly CommandExecutor<RenameSolution> _renameExecutor;
		private readonly CommandExecutor<DeleteSolution> _deleteExecutor;

		public SolutionController(IQueryRepository queries, IApplicationService service,
			CommandExecutor<RenameSolution> renameExecutor,
			CommandExecutor<DeleteSolution> deleteExecutor)
		{
			_queries = queries;
			_service = service;
			_renameExecutor = renameExecutor;
			_deleteExecutor = deleteExecutor;
		}

		public ActionResult Index()
		{
			return RedirectToAction("List");
		}

		public ActionResult List()
		{
			return View(_queries.ListSolutions());
		}

		public ActionResult Create()
		{
			return View("Create");
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Create(CreateSolution command)
		{
			return Validating(command, () =>
			{
				_service.Execute(command);
				return RedirectToAction("List");
			});
		}

		public ActionResult Rename(Guid id)
		{
			Solution solution = _queries.GetSolution(id);
			var mapped = new RenameSolution(id, solution.Name);
			return View(mapped);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Rename(RenameSolution command)
		{
			return Validating(command, () =>
			{
				_renameExecutor.Execute(command);
				return RedirectToAction("List");
			});
		}

		[AcceptVerbs(HttpVerbs.Delete)]
		public ActionResult Delete(Guid id)
		{
			DeleteSolution command = new DeleteSolution(id);
			return Validating(command, () =>
			{
				_deleteExecutor.Execute(command);
				return RedirectToAction("List");
			});
		}
	}
}