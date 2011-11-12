using System;
using System.Web.Mvc;
using Dgg.Cqrs.Sample.Core.Application.DefectHandling.Commands;
using Dgg.Cqrs.Sample.Core.Application.DefectHandling.Queries;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Presentation.Models;
using Dgg.Cqrs.Sample.Core.Presentation.Models.DefectHandling;

namespace Dgg.Cqrs.Sample.Core.Presentation.Controllers
{
	public class IssueController : ControllerBase
	{
		private readonly IQueryRepository _queries;
		private readonly CommandExecutor<OpenIssue> _openExecutor;
		private readonly CommandExecutor<FixIssue> _fixExecutor;
		private readonly CommandExecutor<CloseIssue> _closeExecutor;
		private readonly CommandExecutor<DeleteIssue> _deleteExecutor;

		public IssueController(IQueryRepository queries,
			CommandExecutor<OpenIssue> openExecutor,
			CommandExecutor<FixIssue> fixExecutor,
			CommandExecutor<CloseIssue> closeExecutor,
			CommandExecutor<DeleteIssue> deleteExecutor)
		{
			_queries = queries;
			_openExecutor = openExecutor;
			_fixExecutor = fixExecutor;
			_closeExecutor = closeExecutor;
			_deleteExecutor = deleteExecutor;
		}

		public ActionResult Index()
		{
			return RedirectToAction("List");
		}

		public ActionResult List()
		{
			return View(_queries.ListIssues());
		}

		public ActionResult Open()
		{
			var opening = new IssueOpening();
			setSupportEntitiesForOpening(opening);

			return View(opening);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Open(OpenIssue command)
		{
			return Validating(command, () =>
			{
				_openExecutor.Execute(command);
				return RedirectToAction("List");
			},
			invalidCommand => setSupportEntitiesForOpening(invalidCommand));
		}

		public ActionResult Fix(Guid id)
		{
			Issue issue = _queries.GetIssue(id);

			var empty = new IssueFixing { Title = issue.Title };
			setSupportEntitiesForFixing(empty);
			return View(empty);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Fix(FixIssue command)
		{
			return Validating(command, () =>
			{
				_fixExecutor.Execute(command);
				return RedirectToAction("List");
			},
			invalidModel => resetSupportEntitiesForFixing(invalidModel));
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Close(Guid id)
		{
			var command = new CloseIssue(id);
			return Validating(command, () =>
			{
				_closeExecutor.Execute(command);
				return RedirectToAction("List");
			});
		}

		[AcceptVerbs(HttpVerbs.Delete)]
		public ActionResult Delete(Guid id)
		{
			var command = new DeleteIssue(id);
			return Validating(command, () =>
			{
				_deleteExecutor.Execute(command);
				return RedirectToAction("List");
			});
		}

		private IssueOpening setSupportEntitiesForOpening(IssueOpening opening)
		{
			SupportEntities entities = _queries.GetSupportEntities();

			opening.Solutions = entities.Solutions.ToSelectList(s => s.Id, s => s.Name, _ => false);
			opening.Versions = entities.Versions.ToSelectList(s => s.Id, s => s.Name, _ => false);
			opening.Builds = entities.Builds.ToSelectList(s => s.Id, s => s.Name, _ => false);

			return opening;
		}

		private IssueFixing setSupportEntitiesForFixing(IssueFixing fixing)
		{
			SupportEntities entities = _queries.GetSupportEntities();
			fixing.Versions = entities.Versions.ToSelectList(s => s.Id, s => s.Name, _ => false);
			fixing.Builds = entities.Builds.ToSelectList(s => s.Id, s => s.Name, _ => false);
			return fixing;
		}

		private IssueOpening setSupportEntitiesForOpening(OpenIssue invalidCommand)
		{
			return setSupportEntitiesForOpening(new IssueOpening
			{
				BuildId = invalidCommand.BuildId,
				Description = invalidCommand.Description,
				ExpectedResult = invalidCommand.ExpectedResult,
				SolutionId = invalidCommand.SolutionId,
				StepsToReproduce = invalidCommand.StepsToReproduce,
				Title = invalidCommand.Title,
				VersionId = invalidCommand.VersionId,
				ActualResult = invalidCommand.ActualResult
			});
		}

		private IssueFixing resetSupportEntitiesForFixing(FixIssue invalidCommand)
		{
			return setSupportEntitiesForFixing(new IssueFixing
			{
				FixedInBuildId = invalidCommand.FixedInBuildId,
				FixedInVersionId = invalidCommand.FixedInVersionId,
				Id = invalidCommand.Id,
				Resolution = invalidCommand.Resolution,
				Title = invalidCommand.Title
			});
		}
	}
}
