using System;
using Dgg.Cqrs.Sample.Core.Domain.DefectHandling;
using Dgg.Cqrs.Sample.Core.Tests.Domain.Support;
using NUnit.Framework;
using Testing.Commons.Time;

namespace Dgg.Cqrs.Sample.Core.Tests.Domain.DefectHandling
{
	[TestFixture]
	public class IssueTester : EntityTester
	{
		#region Open

		[Test]
		public void Open_RaisesEvent()
		{
			var solution = new Solution(Guid.Empty, "solution");
			var version = new AppVersion(Guid.Empty, "version");
			var build = new Build(Guid.Empty, "build");
			string title = "title", description = "description", steps = "steps", expected = "expected", actual = "actual";

			bool raised = false;
			OpenIssue.Opening += (sender, e) => { raised = true; };

			Issue.Open(solution, version, build, title, description, steps, expected, actual);

			Assert.That(raised, Is.True);
		}

		[Test]
		public void Open_OnItsOwn_OnlySetsMandatoryValues()
		{
			var solution = new Solution(Guid.Empty, "solution");
			var version = new AppVersion(Guid.Empty, "version");
			var build = new Build(Guid.Empty, "build");
			string title = "title", description = "description", steps = "steps", expected = "expected", actual = "actual";

			DateTimeOffset now = 12.November(2011).At(t => t.MidNight).In(TimeSpan.Zero);
			OpenIssue open = null;
			HappeningIn(now, () =>
				{
					open = Issue.Open(solution, version, build, title, description, steps, expected, actual);
				});

			Assert.That(open.Solution, Is.SameAs(solution));
			Assert.That(open.Version, Is.SameAs(version));
			Assert.That(open.Build, Is.SameAs(build));
			Assert.That(open.Title, Is.EqualTo(title));
			Assert.That(open.Description, Is.EqualTo(description));

			Assert.That(open.StepsToReproduce, Is.Null);
			Assert.That(open.ExpectedResult, Is.Null);
			Assert.That(open.ActualResult, Is.Null);

			Assert.That(open.Opened, Is.EqualTo(default(DateTimeOffset)));

		}

		[Test]
		public void OpenHandler_WhenEventApplied_SetsAllValues()
		{
			var solution = new Solution(Guid.Empty, "solution");
			var version = new AppVersion(Guid.Empty, "version");
			var build = new Build(Guid.Empty, "build");
			string title = "title", description = "description", steps = "steps", expected = "expected", actual = "actual";

			OpenIssue.Opening += EventApplier;

			DateTimeOffset now = 12.November(2011).At(t => t.MidNight).In(TimeSpan.Zero);
			OpenIssue open = null;
			HappeningIn(now, () =>
				{
					open = Issue.Open(solution, version, build, title, description, steps, expected, actual);
				});

			Assert.That(open.Solution, Is.SameAs(solution));
			Assert.That(open.Version, Is.SameAs(version));
			Assert.That(open.Build, Is.SameAs(build));
			Assert.That(open.Title, Is.EqualTo(title));
			Assert.That(open.Description, Is.EqualTo(description));

			Assert.That(open.StepsToReproduce, Is.EqualTo(steps));
			Assert.That(open.ExpectedResult, Is.EqualTo(expected));
			Assert.That(open.ActualResult, Is.EqualTo(actual));

			Assert.That(open.Opened, Is.EqualTo(now));
		}

		#endregion

		#region Fix

		[Test]
		public void Fix_RaisesEvent()
		{
			AppVersion fixingVersion = new AppVersion(Guid.Empty, "fix");
			Build fixingBuild = new Build(Guid.Empty, "fix");
			string resolution = "resolution";

			bool raised = false;
			FixedIssue.Fixing += (sender, e) => { raised = true; };

			Issue.Open(null, null, null, null, null, null, null, null)
				.Fix(fixingVersion, fixingBuild, resolution);

			Assert.That(raised, Is.True);
		}

		[Test]
		public void Fix_OnItsOwn_OnlySetsMandatoryValues()
		{
			AppVersion fixingVersion = new AppVersion(Guid.Empty, "fix");
			Build fixingBuild = new Build(Guid.Empty, "fix");
			string resolution = "resolution";

			DateTimeOffset now = 12.November(2011).At(t => t.MidNight).In(TimeSpan.Zero);
			FixedIssue @fixed = null;
			HappeningIn(now, () =>
				{
					@fixed = Issue.Open(null, null, null, null, null, null, null, null)
						.Fix(fixingVersion, fixingBuild, resolution);
				});

			Assert.That(@fixed.VersionFixed, Is.Null);
			Assert.That(@fixed.BuildFixed, Is.Null);
			Assert.That(@fixed.Resolution, Is.Null);

			Assert.That(@fixed.Fixed, Is.EqualTo(default(DateTimeOffset)));

		}

		[Test]
		public void Fix_WhenEventApplied_SetsAllValues()
		{
			AppVersion fixingVersion = new AppVersion(Guid.Empty, "fix");
			Build fixingBuild = new Build(Guid.Empty, "fix");
			string resolution = "resolution";

			FixedIssue.Fixing += EventApplier;

			DateTimeOffset now = 12.November(2011).At(t => t.MidNight).In(TimeSpan.Zero);
			FixedIssue @fixed = null;
			HappeningIn(now, () =>
				{
					@fixed = Issue.Open(null, null, null, null, null, null, null, null)
						.Fix(fixingVersion, fixingBuild, resolution);
				});

			Assert.That(@fixed.VersionFixed, Is.SameAs(fixingVersion));
			Assert.That(@fixed.BuildFixed, Is.SameAs(fixingBuild));
			Assert.That(@fixed.Resolution, Is.EqualTo(resolution));

			Assert.That(@fixed.Fixed, Is.EqualTo(now));
		}

		#endregion

		#region Close

		[Test]
		public void Close_RaisesEvent()
		{
			bool raised = false;
			ClosedIssue.Closing += (sender, e) => { raised = true; };

			Issue.Open(null, null, null, null, null, null, null, null)
				.Fix(null, null, null)
				.Close();

			Assert.That(raised, Is.True);
		}

		[Test]
		public void Close_OnItsOwn_OnlySetsMandatoryValues()
		{
			DateTimeOffset now = 12.November(2011).At(t => t.MidNight).In(TimeSpan.Zero);
			ClosedIssue closed = null;
			HappeningIn(now, () =>
			{
				closed = Issue.Open(null, null, null, null, null, null, null, null)
					.Fix(null, null, null)
					.Close();
			});

			Assert.That(closed.Closed, Is.EqualTo(default(DateTimeOffset)));

		}

		[Test]
		public void Close_WhenEventApplied_SetsAllValues()
		{
			ClosedIssue.Closing += EventApplier;

			DateTimeOffset now = 12.November(2011).At(t => t.MidNight).In(TimeSpan.Zero);
			ClosedIssue closed = null;
			HappeningIn(now, () =>
			{
				closed = Issue.Open(null, null, null, null, null, null, null, null)
					.Fix(null, null, null)
					.Close();
			});

			Assert.That(closed.Closed, Is.EqualTo(now));
		}

		#endregion

	}
}
