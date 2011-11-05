using System.Collections.Generic;

namespace Dgg.Cqrs.Sample.Core.Presentation.Models.DefectHandling
{
	public class SupportEntities
	{
		public static readonly string TheId = "0";

		public string Id { get { return TheId; } }

		public ICollection<Solution> Solutions { get; set; }

		public static SupportEntities FirstWith(Solution solution)
		{
			return new SupportEntities
			{
				Solutions = new List<Solution> { solution }
			};
		}
	}
}
