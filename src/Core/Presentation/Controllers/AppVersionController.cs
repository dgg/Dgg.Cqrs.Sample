using System;
using System.Web.Mvc;
using Dgg.Cqrs.Sample.Core.Application.Admin.Commands;
using Dgg.Cqrs.Sample.Core.Application.Admin.Queries;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Presentation.Models.Admin;

namespace Dgg.Cqrs.Sample.Core.Presentation.Controllers
{
	public class AppVersionController : ControllerBase
	{
		private readonly IQueryRepository _queries;
		private readonly CommandExecutor<CreateVersion> _createExecutor;
		private readonly CommandExecutor<RenameVersion> _renameExecutor;
		private readonly CommandExecutor<DeleteVersion> _deleteExecutor;
		private readonly CommandExecutor<AssignVersion> _assignExecutor;
		private readonly CommandExecutor<UnAssignVersion> _unAssignExecutor;

		public AppVersionController(IQueryRepository queries,
			CommandExecutor<CreateVersion> createExecutor,
			CommandExecutor<RenameVersion> renameExecutor,
			CommandExecutor<DeleteVersion> deleteExecutor,
			CommandExecutor<AssignVersion> assignExecutor,
			CommandExecutor<UnAssignVersion> unAssignExecutor)
		{
			_queries = queries;
			_createExecutor = createExecutor;
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
			return View(_queries.ListVersions());
		}

		public ActionResult Create()
		{
			return View("Create");
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Create(CreateVersion command)
		{
			return Validating(command, () =>
			{
				_createExecutor.Execute(command);
				return RedirectToAction("List");
			});
		}

		public ActionResult Rename(Guid id)
		{
			AppVersion version = _queries.GetVersion(id);
			var mapped = new RenameVersion(id, version.Name);
			return View(mapped);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Rename(RenameVersion command)
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
			var command = new DeleteVersion(id);
			return Validating(command, () =>
			{
				_deleteExecutor.Execute(command);
				return RedirectToAction("List");
			});
		}

		public ActionResult Assign(Guid id)
		{
			AppVersionAssignation assignation = new AppVersionAssignation(
				_queries.GetVersion(id),
				_queries.ListSolutions());
			return View(assignation);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Assign(AssignVersion command)
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
			var command = new UnAssignVersion(id);
			return Validating(command, () =>
			{
				_unAssignExecutor.Execute(command);
				return RedirectToAction("List");
			});
		}
	}
}
