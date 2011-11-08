using System;
using System.Web.Mvc;
using Dgg.Cqrs.Sample.Core.Application.Admin.Commands;
using Dgg.Cqrs.Sample.Core.Application.Admin.Queries;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Presentation.Models.Admin;

namespace Dgg.Cqrs.Sample.Core.Presentation.Controllers
{
	public class BuildController : ControllerBase
	{
		private readonly IQueryRepository _queries;
		private readonly CommandExecutor<CreateBuild> _createExecutor;
		private readonly CommandExecutor<CreateBulkOfBuilds> _bulkCreateExecutor;
		private readonly CommandExecutor<RenameBuild> _renameExecutor;
		private readonly CommandExecutor<DeleteBuild> _deleteExecutor;
		private readonly CommandExecutor<AssignBuild> _assignExecutor;
		private readonly CommandExecutor<UnAssignBuild> _unAssignExecutor;

		public BuildController(IQueryRepository queries,
			CommandExecutor<CreateBuild> createExecutor,
			CommandExecutor<CreateBulkOfBuilds> bulkCreateExecutor,
			CommandExecutor<RenameBuild> renameExecutor,
			CommandExecutor<DeleteBuild> deleteExecutor,
			CommandExecutor<AssignBuild> assignExecutor,
			CommandExecutor<UnAssignBuild> unAssignExecutor)
		{
			_queries = queries;
			_createExecutor = createExecutor;
			_bulkCreateExecutor = bulkCreateExecutor;
			_renameExecutor = renameExecutor;
			_deleteExecutor = deleteExecutor;
			_assignExecutor = assignExecutor;
			_unAssignExecutor = unAssignExecutor;
		}

		public ActionResult Index()
		{
			return RedirectToAction("List");
		}

		public ActionResult List()
		{
			return View(_queries.ListBuilds());
		}

		public ActionResult Create()
		{
			return View("Create");
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Create(CreateBuild command)
		{
			return Validating(command, () =>
			{
				_createExecutor.Execute(command);
				return RedirectToAction("List");
			});
		}

		public ActionResult BulkCreate()
		{
			return View("BulkCreate");
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult BulkCreate(CreateBulkOfBuilds command)
		{
			return Validating(command, () =>
			{
				_bulkCreateExecutor.Execute(command);
				return RedirectToAction("List");
			});
		}

		public ActionResult Rename(Guid id)
		{
			Build build = _queries.GetBuild(id);
			var mapped = new RenameBuild(id, build.Name);
			return View(mapped);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Rename(RenameBuild command)
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
			var command = new DeleteBuild(id);
			return Validating(command, () =>
			{
				_deleteExecutor.Execute(command);
				return RedirectToAction("List");
			});
		}

		public ActionResult Assign(Guid id)
		{
			var assignation = new BuildAssignation(
				_queries.GetBuild(id),
				_queries.ListSolutions());
			return View(assignation);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Assign(AssignBuild command)
		{
			return Validating(command, () =>
			{
				_assignExecutor.Execute(command);
				return RedirectToAction("List");
			});
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult UnAssign(Guid id)
		{
			var command = new UnAssignBuild(id);
			return Validating(command, () =>
			{
				_unAssignExecutor.Execute(command);
				return RedirectToAction("List");
			});
		}
	}
}
