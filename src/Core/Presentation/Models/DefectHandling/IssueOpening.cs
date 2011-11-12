using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Dgg.Cqrs.Sample.Core.Presentation.Models.DefectHandling
{
	public class IssueOpening
	{
		[Required, StringLength(255, MinimumLength = 1)]
		public string Title { get; set; }

		public IEnumerable<SelectListItem> Solutions { get; set; }

		[DisplayName("Solution")]
		public Guid SolutionId { get; set; }

		public IEnumerable<SelectListItem> Versions { get; set; }

		[DisplayName("Version")]
		public Guid VersionId { get; set; }

		public IEnumerable<SelectListItem> Builds { get; set; }

		[DisplayName("Build")]
		public Guid BuildId { get; set; }

		[Required, StringLength(2048, MinimumLength = 1)]
		public string Description { get; set; }

		[StringLength(2048, MinimumLength = 0)]
		[DisplayName("Steps to Reproduce")]
		public string StepsToReproduce { get; set; }

		[StringLength(2048, MinimumLength = 0)]
		[DisplayName("Expected Result")]
		public string ExpectedResult { get; set; }

		[StringLength(2048, MinimumLength = 0)]
		[DisplayName("Actual Result")]
		public string ActualResult { get; set; }
	}
}