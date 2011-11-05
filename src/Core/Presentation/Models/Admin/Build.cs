namespace Dgg.Cqrs.Sample.Core.Presentation.Models.Admin
{
	public class Build
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public Solution AssignedTo { get; set; }
	}
}
