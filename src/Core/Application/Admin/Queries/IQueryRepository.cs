using System;
using System.Collections.Generic;
using Dgg.Cqrs.Sample.Core.Presentation.Models.Admin;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Queries
{
	public interface IQueryRepository
	{
		IEnumerable<Solution> ListSolutions();
		Solution GetSolution(Guid id);
		/*IEnumerable<AppVersion> ListVersions();
		AppVersion GetVersion(Guid id);
		IEnumerable<Build> ListBuilds();
		Build GetBuild(Guid id);*/
	}
}