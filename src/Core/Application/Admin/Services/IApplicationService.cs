using Dgg.Cqrs.Sample.Core.Application.Admin.Commands;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Services
{
	public interface IApplicationService
	{
		void Execute(CreateSolution command);
	}
}