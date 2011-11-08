using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation.Validators;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Commands
{
	public class AssignBuild : ICommand
	{
		[Obsolete("serialization")]
		public AssignBuild() { }

		public AssignBuild(Guid id, Guid solutionId)
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
