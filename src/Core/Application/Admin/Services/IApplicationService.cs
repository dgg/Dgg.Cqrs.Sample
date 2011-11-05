using Dgg.Cqrs.Sample.Core.Application.Admin.Commands;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Services
{
	public interface IApplicationService
	{
		void Execute(CreateSolution command);
		void Execute(RenameSolution command);
		void Execute(DeleteSolution command);
	}
}