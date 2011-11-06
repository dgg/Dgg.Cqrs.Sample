using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation.Validators;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Commands
{
	public class UnAssignVersion : ICommand
	{
		[Obsolete("serialization")]
		public UnAssignVersion() { }

		public UnAssignVersion(Guid id)
		{
			Id = id;
		}

		[NotEmptyId]
		public Guid Id { get; set; }
	}
}
