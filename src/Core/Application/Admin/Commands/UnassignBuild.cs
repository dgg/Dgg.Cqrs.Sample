using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation.Validators;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Commands
{
	public class UnAssignBuild : ICommand
	{
		[Obsolete("serialization")]
		public UnAssignBuild() { }

		public UnAssignBuild(Guid id)
		{
			Id = id;
		}

		[NotEmptyId]
		public Guid Id { get; set; }
	}
}
