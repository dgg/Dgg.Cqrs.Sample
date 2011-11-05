using System;
using System.ComponentModel;
using Dgg.Cqrs.Sample.Core.Presentation.Models.Admin;

namespace Dgg.Cqrs.Sample.Core.Presentation.Models.DefectHandling
{
	public class Issue
	{
		public string Id { get; set; }

		public IssueStatus Status { get; set; }

		public Solution Solution { get; set; }
		public AppVersion Version { get; set; }
		public Build Build { get; set; }

		public string Title { get; set; }
		public string Description { get; set; }
		[DisplayName("Steps to Reproduce")]
		public string StepsToReproduce { get; set; }
		[DisplayName("Expected Result")]
		public string ExpectedResult { get; set; }
		[DisplayName("Actual Result")]
		public string ActualResult { get; set; }

		public DateTimeOffset Opened { get; set; }

		public AppVersion VersionFixed { get; set; }
		public Build BuildFixed { get; set; }
		public DateTimeOffset? Fixed { get; set; }

		public DateTimeOffset? Closed { get; set; }
	}
}
