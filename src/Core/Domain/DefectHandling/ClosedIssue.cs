using System;
using Dgg.Cqrs.Sample.Core.Domain.DefectHandling.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;

namespace Dgg.Cqrs.Sample.Core.Domain.DefectHandling
{
	public class ClosedIssue : Issue
	{
		internal ClosedIssue(FixedIssue self) : base(self.Id, self.Solution, self.Version, self.Build, self.Title, self.Description)
		{
			StepsToReproduce = self.StepsToReproduce;
			ExpectedResult = self.ExpectedResult;
			ActualResult = self.ActualResult;
			Opened = self.Opened;

			VersionFixed = self.VersionFixed;
			BuildFixed = self.BuildFixed;
			Resolution = self.Resolution;
			Fixed = self.Fixed;

			OnClosing(Time.UtcNow);
		}

		/// <summary>
		/// STATIC, do unsubscribe from it
		/// </summary>
		public static event DomainEventHandler<IssueClosed> Closing;
		// not virtual as is called from the ctor
		protected void OnClosing(DateTimeOffset closed)
		{
			DomainEventHandler<IssueClosed> handler = Closing;
			if (handler != null) handler(this, new DomainEventEventArgs<IssueClosed>(
				new IssueClosed(Id) {  Closed = closed },
				doClose));
		}

		private bool doClose(IssueClosed e)
		{
			Closed = e.Closed;
			return true;
		}

		public DateTimeOffset Opened { get; private set; }

		public AppVersion VersionFixed { get; private set; }
		public Build BuildFixed { get; private set; }
		public string Resolution { get; private set; }

		public DateTimeOffset Fixed { get; private set; }

		public DateTimeOffset Closed { get; private set; }
	}
}