using System;
using Dgg.Cqrs.Sample.Core.Infrastructure;

namespace Dgg.Cqrs.Sample.Core.Domain.Admin
{
	public class AppVersion
	{
		public AppVersion(string name) : this(Comb.Generate(), name) { }
		public AppVersion(Guid id, string name)
		{
			Id = id;
			Name = name;
		}

		public Guid Id { get; private set; }
		public Solution Solution { get; private set; }
		public string Name { get; private set; }

		public AppVersion Rename(string newName)
		{
			Name = newName;
			return this;
		}
		public AppVersion Assign(Solution solution)
		{
			Solution = solution;
			return this;
		}
	}
}
