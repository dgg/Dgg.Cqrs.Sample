using System;

namespace Dgg.Cqrs.Sample.Core.Domain.DefectHandling
{
	public class Build
	{
		public Build(Guid id, string name)
		{
			Id = id;
			Name = name;
		}

		public Guid Id { get; private set; }
		public string Name { get; set; }
	}
}