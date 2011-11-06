using System;
using System.Collections.Generic;
using System.Linq;
using Dgg.Cqrs.Sample.Core.Application.Admin.Commands;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation;
using Raven.Client;
using Solution = Dgg.Cqrs.Sample.Core.Domain.Admin.Solution;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Services
{
	public class ApplicationService : IApplicationService
	{
		private readonly IValidationService _validation;
		private readonly IDocumentSession _models;
		private readonly ISnapshotSession _snapshots;

		public ApplicationService(IValidationService validation, ISnapshotSession snapshots, IDocumentSession models)
		{
			_validation = validation;
			_snapshots = snapshots;
			_models = models;
		}

		#region Create Solution

		public void Execute(CreateSolution command)
		{
			_validation.AssertValidity(command);

			try
			{
				// this bounded context (Admin)
				Solution solution = createSnapshot(command);
				updateModelForCreation(solution);

				// another bounded context (DefectHandling)
				createHandlingSnapshot(solution);
				updateHandlingModel(solution);

				_models.SaveChanges();
			}
			catch (Exception)
			{
				_snapshots.RollbackChanges();
				throw;
			}
		}

		private Solution createSnapshot(CreateSolution command)
		{
			Solution solution = new Solution(command.Name);
			_snapshots.Save(solution);
			return solution;
		}

		private void updateModelForCreation(Solution e)
		{
			_models.Store(new Presentation.Models.Admin.Solution {Id = e.Id.ToString(), Name = e.Name});
		}

		private void createHandlingSnapshot(Solution e)
		{
			Domain.DefectHandling.Solution ds = new Domain.DefectHandling.Solution(e.Id, e.Name);
			_snapshots.Save(ds);
		}

		private void updateHandlingModel(Solution e)
		{
			if (!documentExists()) createSupportEntitiesFor(e);
			else updateSupportEntitiesForCreation(e);
		}

		private bool documentExists()
		{
			return _models.Query<Presentation.Models.DefectHandling.SupportEntities>().Any();
		}

		private void createSupportEntitiesFor(Solution e)
		{
			_models.Store(Presentation.Models.DefectHandling.SupportEntities.FirstWith(
				new Presentation.Models.DefectHandling.Solution {Id = e.Id.ToString(), Name = e.Name}));
		}

		private void updateSupportEntitiesForCreation(Solution e)
		{
			// is not optimal to get the whole document for adding to an internal array, but partial updates did not work...
			var entities = _models.Single<Presentation.Models.DefectHandling.SupportEntities>(
				Presentation.Models.DefectHandling.SupportEntities.TheId);
			var solutions = entities.Solutions ?? new List<Presentation.Models.DefectHandling.Solution>();
			solutions.Add(new Presentation.Models.DefectHandling.Solution
			              	{
			              		Id = e.Id.ToString(),
			              		Name = e.Name
			              	});
			entities.Solutions = solutions;
		}

		#endregion

		#region Rename Solution

		public void Execute(RenameSolution command)
		{
			_validation.AssertValidity(command);

			try
			{
				Solution renamed = renameSnapshot(command);
				renameModel(renamed);
				renameAssignedModels(renamed);

				// another bounded context (DefectHandling)
				renameHandlingSnapshot(renamed);
				renameSupportEntities(renamed);
				renameHandlingAssignedModels(renamed);

				_models.SaveChanges();
			}
			catch (Exception)
			{
				_snapshots.RollbackChanges();
				throw;
			}
		}

		public Solution renameSnapshot(RenameSolution command)
		{
			Solution old = _snapshots.Single<Solution>(s => s.Id == command.Id);
			old.Rename(command.NewName);
			_snapshots.Save(old);
			return old;
		}

		private void renameModel(Solution e)
		{
			var model = _models.Single<Presentation.Models.Admin.Solution>(e.Id);
			model.Name = e.Name;
		}

		private void renameAssignedModels(Solution e)
		{
			string indexName = typeof (Queries.VersionsAssignedToSolution).Name;
			Presentation.Models.Admin.AppVersion[] assignedVersions = _models.Query<Presentation.Models.Admin.AppVersion>(indexName)
				.Where(v => v.AssignedTo.Id == e.Id.ToString())
				.ToArray();
			Array.ForEach(assignedVersions, version => version.AssignedTo.Name = e.Name);

			indexName = typeof(Queries.BuildsAssignedToSolution).Name;
			Presentation.Models.Admin.Build[] assignedBuilds = _models.Query<Presentation.Models.Admin.Build>(indexName)
				.Where(v => v.AssignedTo.Id == e.Id.ToString())
				.ToArray();

			Array.ForEach(assignedBuilds, build => build.AssignedTo.Name = e.Name);
		}

		private void renameHandlingSnapshot(Solution e)
		{
			var solution = _snapshots.Single<Domain.DefectHandling.Solution>(s => s.Id.Equals(e.Id));
			solution.Name = e.Name;
			_snapshots.Save(solution);
		}

		private void renameSupportEntities(Solution e)
		{
			// is not optimal to get the whole document for adding to an internal array, but partial updates did not work...
			var entities = _models.Single<Presentation.Models.DefectHandling.SupportEntities>(
				Presentation.Models.DefectHandling.SupportEntities.TheId);
			var toBeRenamed = entities.Solutions.Single(s => s.Id.Equals(e.Id.ToString(), StringComparison.Ordinal));
			toBeRenamed.Name = e.Name;
		}

		private void renameHandlingAssignedModels(Solution e)
		{
			var issues = _models.Query<Presentation.Models.DefectHandling.Issue, DefectHandling.Queries.IssuesAssignedToSolution>()
				.Where(i => i.Solution.Id == e.Id.ToString());
			foreach (var issue in issues)
			{
				issue.Solution.Name = e.Name;
			}
		}

		#endregion

		#region Delete Solution

		public void Execute(DeleteSolution command)
		{
			_validation.AssertValidity(command);

			try
			{
				deleteSnapshot(command);
				deleteModel(command);
				deleteAssignedModels(command);

				// another bounded context (DefectHandling)
				deleteHandlingSnapshot(command);
				deleteHandlingAssignedSnapshot(command);
				deleteSupportEntity(command);
				deleteHandlingAssignedModels(command);

				_models.SaveChanges();
			}
			catch (Exception)
			{
				_snapshots.RollbackChanges();
				throw;
			}
		}

		private void deleteSnapshot(DeleteSolution cmd)
		{
			_snapshots.Delete<Solution>(s => s.Id == cmd.Id);
		}

		private void deleteModel(DeleteSolution cmd)
		{
			var toBeDeleted = _models.Single<Presentation.Models.Admin.Solution>(cmd.Id);
			_models.Delete(toBeDeleted);
		}

		private void deleteAssignedModels(DeleteSolution cmd)
		{
			Presentation.Models.Admin.AppVersion[] assignedVersions = _models.Query<Presentation.Models.Admin.AppVersion, Queries.VersionsAssignedToSolution>()
				.Where(v => v.AssignedTo.Id == cmd.Id.ToString())
				.ToArray();

			Array.ForEach(assignedVersions, version => version.AssignedTo = null);

			Presentation.Models.Admin.Build[] assignedBuilds = _models.Query<Presentation.Models.Admin.Build, Queries.BuildsAssignedToSolution>()
				.Where(v => v.AssignedTo.Id == cmd.Id.ToString())
				.ToArray();

			Array.ForEach(assignedBuilds, build => build.AssignedTo = null);
		}

		private void deleteHandlingSnapshot(DeleteSolution cmd)
		{
			_snapshots.Delete<Domain.DefectHandling.Solution>(s => s.Id.Equals(cmd.Id));
		}

		private void deleteHandlingAssignedSnapshot(DeleteSolution cmd)
		{
			_snapshots.Delete<Domain.DefectHandling.Issue>(i => i.Solution.Id == cmd.Id);
		}

		private void deleteSupportEntity(DeleteSolution cmd)
		{
			var entities = _models.Single<Presentation.Models.DefectHandling.SupportEntities>(
				Presentation.Models.DefectHandling.SupportEntities.TheId);
			var toBeDeleted = entities.Solutions.Single(s => s.Id.Equals(cmd.Id.ToString(), StringComparison.Ordinal));
			entities.Solutions.Remove(toBeDeleted);
		}

		private void deleteHandlingAssignedModels(DeleteSolution cmd)
		{
			Presentation.Models.DefectHandling.Issue[] relatedIssues = _models.Query<Presentation.Models.DefectHandling.Issue>()
				.Where(i => i.Solution.Id == cmd.Id.ToString())
				.ToArray();

			Array.ForEach(relatedIssues, i => _models.Delete(i));
		}

		#endregion
	}
}
