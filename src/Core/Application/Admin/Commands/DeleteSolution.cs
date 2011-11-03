using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation.Validators;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Commands
{
	public class DeleteSolution : ICommand
	{
		[Obsolete("serialization")]
		public DeleteSolution() { }

		public DeleteSolution(Guid id)
		{
			Id = id;
		}

		[NotEmptyId]
		public Guid Id { get; set;}
	}
}
