using System;
using Dgg.Cqrs.Sample.Core.Application.Admin.Commands;
using Dgg.Cqrs.Sample.Core.Domain.Admin;
using Dgg.Cqrs.Sample.Core.Domain.Admin.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Services.Executors
{
	public class AssignBuildExecutor : CommandExecutor<AssignBuild>
	{
		private readonly IValidationService _validator;
		private readonly ISnapshotSession _snapshots;
		private readonly IEventSession _events;
		private readonly IDocumentSession _models;
		private readonly EventBus<BuildAssigned> _bus;

		public AssignBuildExecutor(IValidationService validator, ISnapshotSession snapshots, IEventSession events, IDocumentSession models, EventBus<BuildAssigned> bus)
		{
			_validator = validator;
			_snapshots = snapshots;
			_events = events;
			_models = models;
			_bus = bus;
		}

		public override void Execute(AssignBuild cmd)
		{
			_validator.AssertValidity(cmd);

			if (isAssignationIndeed(cmd))
			{
				try
				{
					Build build = _snapshots.Single<Build>(v => v.Id == cmd.Id);
					build.Assigning += (sender, e) =>
					{
						_bus.Subscribe(e.EntityHandler);
						_bus.Publish(e.Event);
					};

					Solution solution = _snapshots.Single<Solution>(s => s.Id == cmd.SolutionId);
					build.Assign(solution);
					_snapshots.Save(build);

					_models.SaveChanges();
				}
				catch(Exception)
				{
					_events.RollbackChanges();
					_snapshots.RollbackChanges();
					throw;
				}
			}
		}

		private static bool isAssignationIndeed(AssignBuild cmd)
		{
			return cmd != null && cmd.SolutionId != Guid.Empty;
		}
	}
}
