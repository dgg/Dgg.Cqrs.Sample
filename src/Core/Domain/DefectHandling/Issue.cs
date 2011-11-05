using System;
using Dgg.Cqrs.Sample.Core.Infrastructure;

namespace Dgg.Cqrs.Sample.Core.Domain.DefectHandling
{
	public abstract class Issue
	{
		protected Issue(Solution solution, AppVersion version, Build build, string title, string description) :
			this(Comb.Generate(), solution, version, build, title, description) { }

		protected Issue(Guid id, Solution solution, AppVersion version, Build build, string title, string description)
		{
			Id = id;
			Solution = solution;
			Version = version;
			Build = build;
			Title = title;
			Description = description;
		}

		public Guid Id { get; protected set; }
		public Solution Solution { get; protected set; }
		public AppVersion Version { get; protected set; }
		public Build Build { get; protected set; }
		public string Title { get; protected set; }
		public string Description { get; protected set; }

		public string StepsToReproduce { get; set; }
		public string ExpectedResult { get; set; }
		public string ActualResult { get; set; }

		/*public static OpenIssue Open(Solution solution, AppVersion version, Build build, string title, string description, string stepsToReproduce, string expectedResult, string actualResult)
		{
			return new OpenIssue(solution, version, build, title, description, stepsToReproduce, expectedResult, actualResult);
		}*/
	}
}