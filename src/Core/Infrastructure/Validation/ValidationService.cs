using System.ComponentModel.DataAnnotations;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Validation
{
	public class ValidationService : IValidationService
	{
		public void AssertValidity<T>(T command) where T : class
		{
			var ctx = new ValidationContext(command, null, null);
			Validator.ValidateObject(command, ctx, true);
		}

		public void AssertValidity<T>(T command, params string[] validationGroups) where T : class
		{
			var ctx = new ValidationContext(command, null, null);
			Validator.ValidateObject(command, ctx, true);
		}
	}
}