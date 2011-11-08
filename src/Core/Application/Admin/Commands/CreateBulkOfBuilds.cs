using System;
using System.ComponentModel.DataAnnotations;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation.Validators;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Commands
{
	public class CreateBulkOfBuilds : ICommand
	{
		[Obsolete("serialization")]
		public CreateBulkOfBuilds() { }

		public CreateBulkOfBuilds(string prefix, int rangeStart, int rangeEnd)
		{
			Prefix = prefix;
			RangeStart = rangeStart;
			RangeEnd = rangeEnd;
		}

		[Required, StringLength(128, MinimumLength = 1)]
		public string Prefix { get; set; }
		[StringLength(128)]
		public string Suffix { get; set; }
		[Range(0, 3000), RangeValidator("RangeEnd")]
		public int RangeStart { get; set; }
		[Range(0, 3000)]
		public int RangeEnd { get; set; }
	}
}
