﻿using System.Linq;
using Dgg.Cqrs.Sample.Core.Presentation.Models.DefectHandling;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Dgg.Cqrs.Sample.Core.Application.DefectHandling.Queries
{
	public class IssuesAssignedToVersion : AbstractIndexCreationTask
	{
		public override IndexDefinition CreateIndexDefinition()
		{
			return new IndexDefinitionBuilder<Issue>
			{
				Map = Issues =>
					from issue in Issues
					select new { issue.Version.Id }
			}.ToIndexDefinition(Conventions);
		}
	}
}
