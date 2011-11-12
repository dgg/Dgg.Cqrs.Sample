using System;
using System.Collections.Generic;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Presentation.Models.DefectHandling;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.DefectHandling.Queries
{
	public class QueryRepository : IQueryRepository
	{
		private readonly IDocumentSession _session;

		public QueryRepository(IDocumentSession session)
		{
			_session = session;
		}

		public IEnumerable<Issue> ListIssues()
		{
			return _session.FindAll<Issue>();
		}

		public SupportEntities GetSupportEntities()
		{
			return _session.Single<SupportEntities>(SupportEntities.TheId);
		}

		public Issue GetIssue(Guid id)
		{
			return _session.Single<Issue>(id);
		}
	}
}