using System.Linq;
using Dgg.Cqrs.Sample.Core.Presentation.Models.Admin;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Queries
{
	public class VersionsAssignedToSolution : AbstractIndexCreationTask
	{
		public override IndexDefinition CreateIndexDefinition()
		{
			return new IndexDefinitionBuilder<AppVersion>
			{
				Map = AppVersions =>
					from version in AppVersions
					where version.AssignedTo != null
					select new { version.AssignedTo.Id }
			}.ToIndexDefinition(Conventions);
		}
	}
}
