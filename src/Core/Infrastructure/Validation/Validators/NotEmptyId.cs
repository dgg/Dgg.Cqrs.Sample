using System;
using System.ComponentModel.DataAnnotations;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Validation.Validators
{
	public sealed class NotEmptyId : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			return !Guid.Empty.Equals((Guid) value);
		}
	}
}
