using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation.Validators;

namespace Dgg.Cqrs.Sample.Core.Presentation.Models.DefectHandling
{
	public class IssueFixing
	{
		[NotEmptyId]
		public Guid Id { get; set; }

		public IEnumerable<SelectListItem> Versions { get; set; }

		[NotEmptyId]
		[DisplayName("Fixed in Version")]
		public Guid FixedInVersionId { get; set; }

		public IEnumerable<SelectListItem> Builds { get; set; }

		[NotEmptyId]
		[DisplayName("Fixed in Build")]
		public Guid FixedInBuildId { get; set; }

		[Required, StringLength(2048, MinimumLength = 1)]
		public string Resolution { get; set; }

		public string Title { get; set; }
	}
}
