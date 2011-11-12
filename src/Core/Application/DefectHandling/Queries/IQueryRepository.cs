using System;
using System.Collections.Generic;
using Dgg.Cqrs.Sample.Core.Presentation.Models.DefectHandling;

namespace Dgg.Cqrs.Sample.Core.Application.DefectHandling.Queries
{
	public interface IQueryRepository
	{
		IEnumerable<Issue> ListIssues();
		SupportEntities GetSupportEntities();
		Issue GetIssue(Guid id);
	}
}
