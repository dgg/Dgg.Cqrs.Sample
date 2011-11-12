using System;
using System.ComponentModel.DataAnnotations;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation.Validators;

namespace Dgg.Cqrs.Sample.Core.Application.DefectHandling.Commands
{
	public class OpenIssue : ICommand
	{
		public OpenIssue() { }

		public OpenIssue(string title, Guid solutionId, Guid versionId, Guid buildId, string description, string stepsToReproduce, string expectedResult, string actualResult)
		{
			Title = title;
			SolutionId = solutionId;
			VersionId = versionId;
			BuildId = buildId;
			Description = description;
			StepsToReproduce = stepsToReproduce;
			ExpectedResult = expectedResult;
			ActualResult = actualResult;
		}

		[Required, StringLength(255, MinimumLength = 1)]
		public string Title { get; set; }

		[NotEmptyId]
		public Guid SolutionId { get; set; }
		[NotEmptyId]
		public Guid VersionId { get; set; }
		[NotEmptyId]
		public Guid BuildId { get; set; }

		[Required, StringLength(2048, MinimumLength = 1)]
		public string Description { get; set; }

		[StringLength(2048, MinimumLength = 0)]
		public string StepsToReproduce { get; set; }
		[StringLength(2048, MinimumLength = 0)]
		public string ExpectedResult { get; set; }
		[StringLength(2048, MinimumLength = 0)]
		public string ActualResult { get; set; }
	}
}
