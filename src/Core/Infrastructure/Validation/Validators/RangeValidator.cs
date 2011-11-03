using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Validation.Validators
{
	/// <summary>
	/// Place over the property that represents the start of the range
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class RangeValidatorAttribute : ValidationAttribute
	{
		private readonly string _endName;
		private int _start, _end;
		public RangeValidatorAttribute(string endName)
		{
			_endName = endName;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			_start = (int) value;
			PropertyInfo end = validationContext.ObjectType.GetProperty(_endName);
			_end = (int)end.GetValue(validationContext.ObjectInstance, new object[] {});
			
			return base.IsValid(value, validationContext);
		}

		public override bool IsValid(object value)
		{
			return _start <= _end;
		}
	}
}
