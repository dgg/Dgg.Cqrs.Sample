using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation.Validators;

namespace Dgg.Cqrs.Sample.Core.Application.DefectHandling.Commands
{
	public class CloseIssue : ICommand
	{
		public CloseIssue() { }

		public CloseIssue(Guid issueId)
		{
			Id = issueId;
		}

		[NotEmptyId]
		public Guid Id { get; set; }
	}
}
