using System;
using Dgg.Cqrs.Sample.Core.Domain.DefectHandling.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;

namespace Dgg.Cqrs.Sample.Core.Domain.DefectHandling
{
	public class OpenIssue : Issue
	{
		protected internal OpenIssue(Solution solution, AppVersion version, Build build, string title, string description, string stepsToReproduce, string expectedResult, string actualResult)
			: base(solution, version, build, title, description)
		{
			OnOpening(Comb.Generate(), solution, version, build, title, description, stepsToReproduce, expectedResult, actualResult, Time.UtcNow);
		}

		public bool doOpen(IssueOpened e)
		{
			Id = e.ReceiverId;
			Solution = e.Solution;
			Version = e.Version;
			Build = e.Build;
			Title = e.Title;
			Description = e.Description;
			StepsToReproduce = e.StepsToReproduce;
			ExpectedResult = e.ExpectedResult;
			ActualResult = e.ActualResult;
			Opened = e.Opened;

			return true;
		}

		/// <summary>
		/// STATIC, do unsubscribe from it
		/// </summary>
		public static event DomainEventHandler<IssueOpened> Opening;
		// not virtual as is called from the ctor
		protected void OnOpening(Guid id, Solution solution, AppVersion version, Build build, string title, string description, string stepsToReproduce, string expectedResult, string actualResult, DateTimeOffset opened)
		{
			DomainEventHandler<IssueOpened> handler = Opening;
			if (handler != null) handler(this, new DomainEventEventArgs<IssueOpened>(
				new IssueOpened(id) { Solution = solution, Version = version, Build = build, Title = title, Description = description, StepsToReproduce = stepsToReproduce, ExpectedResult = expectedResult, ActualResult = actualResult, Opened = opened },
				doOpen));
		}

		public DateTimeOffset Opened { get; private set; }

		public FixedIssue Fix(AppVersion versionFixed, Build buildFixed, string resolution)
		{
			return new FixedIssue(this, versionFixed, buildFixed, resolution);
		}
	}
}