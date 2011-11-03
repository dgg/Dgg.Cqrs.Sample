using System;
using System.Collections.Generic;
using Dgg.Cqrs.Sample.Core.Infrastructure.Data;
using Dgg.Cqrs.Sample.Core.Presentation.Models.Admin;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Queries
{
	public class QueryRepository : IQueryRepository
	{
		private readonly IDocumentSession _session;

		public QueryRepository(IDocumentSession session)
		{
			_session = session;
		}

		// TODO: implement pagination
		public IEnumerable<Solution> ListSolutions()
		{
			return _session.FindAll<Solution>();
		}

		public Solution GetSolution(Guid id)
		{
			return _session.Single<Solution>(id);
		}

		/*public IEnumerable<AppVersion> ListVersions()
		{
			return _session.FindAll<AppVersion>();
		}

		public AppVersion GetVersion(Guid id)
		{
			return _session.Single<AppVersion>(id);
		}

		public IEnumerable<Build> ListBuilds()
		{
			return _session.FindAll<Build>();
		}

		public Build GetBuild(Guid id)
		{
			return _session.Single<Build>(id);
		}*/
	}
}
