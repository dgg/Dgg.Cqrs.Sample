using System.ComponentModel.DataAnnotations;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Commands
{
	public class CreateSolution
	{
		[Required, StringLength(255, MinimumLength = 1)]
		public string Name { get; set; }
	}
}
