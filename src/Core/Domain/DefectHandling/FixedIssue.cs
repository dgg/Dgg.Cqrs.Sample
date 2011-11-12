using System;
using Dgg.Anug.Cqrs.Core.Domain.DefectHandling.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;

namespace Dgg.Cqrs.Sample.Core.Domain.DefectHandling
{
	public class FixedIssue : Issue
	{
		internal FixedIssue(OpenIssue self, AppVersion versionFixed, Build buildFixed, string resolution) : base(self.Id, self.Solution, self.Version, self.Build, self.Title, self.Description)
		{
			StepsToReproduce = self.StepsToReproduce;
			ExpectedResult = self.ExpectedResult;
			ActualResult = self.ActualResult;
			Opened = self.Opened;

			OnFixing(versionFixed, buildFixed, resolution, Time.UtcNow);
		}

		/// <summary>
		/// STATIC, do unsubscribe from it
		/// </summary>
		public static event DomainEventHandler<IssueFixed> Fixing;
		// not virtual as is called from the ctor
		protected void OnFixing(AppVersion versionFixed, Build buildFixed, string resolution, DateTimeOffset @fixed)
		{
			DomainEventHandler<IssueFixed> handler = Fixing;
			if (handler != null) handler(this, new DomainEventEventArgs<IssueFixed>(
				new IssueFixed(Id) {  VersionFixed = versionFixed, BuildFixed = buildFixed, Resolution = resolution, Fixed = @fixed },
				doFix));
		}

		private bool doFix(IssueFixed e)
		{
			VersionFixed = e.VersionFixed;
			BuildFixed = e.BuildFixed;
			Resolution = e.Resolution;
			Fixed = e.Fixed;

			return true;
		}

		public ClosedIssue Close()
		{
			return new ClosedIssue(this);
		}

		public DateTimeOffset Opened { get; private set; }

		public AppVersion VersionFixed { get; private set; }
		public Build BuildFixed { get; private set; }
		public string Resolution { get;private set; }

		public DateTimeOffset Fixed { get; private set; }
	}
}