using System;
using Dgg.Cqrs.Sample.Core.Infrastructure;

namespace Dgg.Cqrs.Sample.Core.Domain.Admin
{
	public class Solution
	{
		public Solution(string name) : this(Comb.Generate(), name) { }

		public Solution(Guid id, string name)
		{
			Id = id;
			Name = name;
		}

		public Guid Id { get; private set; }
		public string Name { get; private set; }

		public Solution Rename(string newName)
		{
			Name = newName;
			return this;
		}
	}
}