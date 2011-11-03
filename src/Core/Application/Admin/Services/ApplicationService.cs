using System;
using Dgg.Cqrs.Sample.Core.Domain.Admin;
using Dgg.Cqrs.Sample.Core.Domain.Admin.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Services
{
	public class ApplicationService : IApplicationService
	{
		private readonly IValidationService _validation;
		private readonly IDocumentSession _modelSession;
		private readonly ISnapshotSession _snapshotSession;
		private readonly EventBus<SolutionCreated> _bus;

		public ApplicationService(IValidationService validation, ISnapshotSession snapshotSession, EventBus<SolutionCreated> bus, IDocumentSession modelSession)
		{
			_validation = validation;
			_snapshotSession = snapshotSession;
			_bus = bus;
			_modelSession = modelSession;
		}

		public void Execute(Commands.CreateSolution command)
		{
			_validation.AssertValidity(command);

			try
			{
				Solution solution = new Solution(command.Name);
				_snapshotSession.Save(solution);

				// odd, the domain object should raise it
				SolutionCreated e = new SolutionCreated(solution.Id) { Name = command.Name };
				_bus.Publish(e);

				_modelSession.SaveChanges();
			}
			catch (Exception)
			{
				_snapshotSession.RollbackChanges();
				throw;
			}
		}
	}
}
