using System.Collections.Generic;

namespace Dgg.Cqrs.Sample.Core.Presentation.Models.DefectHandling
{
	public class SupportEntities
	{
		public static readonly string TheId = "0";

		public string Id { get { return TheId; } }

		public ICollection<Solution> Solutions { get; set; }
		public ICollection<AppVersion> Versions { get; set; }
		public ICollection<Build> Builds { get; set; }

		public static SupportEntities FirstWith(Solution solution)
		{
			return new SupportEntities
			{
				Solutions = new List<Solution> { solution }
			};
		}

		public static SupportEntities FirstWith(AppVersion version)
		{
			return new SupportEntities
			{
				Versions = new List<AppVersion> { version }
			};
		}

		public static SupportEntities FirstWith(Build build)
		{
			return new SupportEntities
			{
				Builds = new List<Build> { build }
			};
		}
	}
}
