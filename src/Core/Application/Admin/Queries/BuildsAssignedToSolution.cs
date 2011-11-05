using System.Linq;
using Dgg.Cqrs.Sample.Core.Presentation.Models.Admin;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Queries
{
	public class BuildsAssignedToSolution : AbstractIndexCreationTask
	{
		public override IndexDefinition CreateIndexDefinition()
		{
			return new IndexDefinitionBuilder<Build>
			{
				Map = Builds =>
					from build in Builds
					where build.AssignedTo != null
					select new { build.AssignedTo.Id }
			}.ToIndexDefinition(Conventions);
		}
	}
}