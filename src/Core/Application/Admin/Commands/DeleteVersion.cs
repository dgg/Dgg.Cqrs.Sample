using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation.Validators;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Commands
{
	public class DeleteVersion : ICommand
	{
		[Obsolete("serialization")]
		public DeleteVersion() { }

		public DeleteVersion(Guid id)
		{
			Id = id;
		}

		[NotEmptyId]
		public Guid Id { get; set;}
	}
}
