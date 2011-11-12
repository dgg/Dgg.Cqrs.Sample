using System.ComponentModel.DataAnnotations;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Commands
{
	public class CreateSolution : ICommand
	{
		[Required, StringLength(255, MinimumLength = 1)]
		public string Name { get; set; }
	}
}
