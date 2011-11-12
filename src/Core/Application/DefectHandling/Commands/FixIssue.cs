using System;
using System.ComponentModel.DataAnnotations;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation.Validators;

namespace Dgg.Cqrs.Sample.Core.Application.DefectHandling.Commands
{
	public class FixIssue : ICommand
	{
		[Obsolete("serialization")]
		public FixIssue() { }

		public FixIssue(Guid id): this(id, Guid.Empty, Guid.Empty, null) { }

		public FixIssue(Guid id, Guid fixedInVersionId, Guid fixedInBuildId, string resolution)
		{
			Id = id;
			FixedInVersionId = fixedInVersionId;
			FixedInBuildId = fixedInBuildId;
			Resolution = resolution;
		}

		[NotEmptyId]
		public Guid Id { get; set; }

		[NotEmptyId]
		public Guid FixedInVersionId { get; set; }

		[NotEmptyId]
		public Guid FixedInBuildId { get; set; }

		[Required, StringLength(2048, MinimumLength = 1)]
		public string Resolution { get; set; }

		public string Title { get; set; }
	}
}
