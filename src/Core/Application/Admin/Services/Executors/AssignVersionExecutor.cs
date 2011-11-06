using System;
using Dgg.Cqrs.Sample.Core.Application.Admin.Commands;
using Dgg.Cqrs.Sample.Core.Domain.Admin;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Services.Executors
{
	public class AssignVersionExecutor : CommandExecutor<AssignVersion>
	{
		private readonly IValidationService _validator;
		private readonly ISnapshotSession _snapshots;
		private readonly IDocumentSession _models;

		public AssignVersionExecutor(IValidationService validator, ISnapshotSession snapshots, IDocumentSession models)
		{
			_validator = validator;
			_snapshots = snapshots;
			_models = models;
		}

		public override void Execute(AssignVersion cmd)
		{
			_validator.AssertValidity(cmd);

			if (isAssignationIndeed(cmd))
			{
				try
				{
					AppVersion assigned = assignSnapshot(cmd);
					udpateModel(assigned);
					
					_models.SaveChanges();
				}
				catch(Exception)
				{
					_snapshots.RollbackChanges();
					throw;
				}
			}
		}

		private static bool isAssignationIndeed(AssignVersion cmd)
		{
			return cmd != null && cmd.SolutionId != Guid.Empty;
		}

		private AppVersion assignSnapshot(AssignVersion cmd)
		{
			AppVersion version = _snapshots.Single<AppVersion>(v => v.Id == cmd.Id);
			Solution solution = _snapshots.Single<Solution>(s => s.Id == cmd.SolutionId);
			version.Assign(solution);
			_snapshots.Save(version);
			return version;
		}

		private void udpateModel(AppVersion e)
		{
			var model = _models.Single<Presentation.Models.Admin.AppVersion>(e.Id);
			var solution = e.Solution;
			model.AssignedTo = solution != null ?
				new Presentation.Models.Admin.Solution { Id = e.Solution.Id.ToString(), Name = e.Solution.Name } :
				null;
		}
	}
}
