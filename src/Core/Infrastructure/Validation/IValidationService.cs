namespace Dgg.Cqrs.Sample.Core.Infrastructure.Validation
{
	public interface IValidationService
	{
		void AssertValidity<T>(T command) where T: class;
		void AssertValidity<T>(T command, params string[] validationGroups) where T: class;
	}
}
