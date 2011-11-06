using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation.Validators;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Commands
{
	public class AssignVersion : ICommand
	{
		[Obsolete("serialization")]
		public AssignVersion() { }

		public AssignVersion(Guid id, Guid solutionId)
		{
			Id = id;
			SolutionId = solutionId;
		}

		[NotEmptyId]
		public Guid Id { get; set; }
		// can be empty when unassigning
		public Guid SolutionId { get; set; }
	}
}
