using System;
using Dgg.Cqrs.Sample.Core.Domain.Admin.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;

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
			OnRenaming(Name, newName);
			return this;
		}
		private bool doRename(SolutionRenamed e)
		{
			Name = e.NewName;
			return false;
		}

		public event DomainEventHandler<SolutionRenamed> Renaming;
		protected virtual void OnRenaming(string oldName, string newName)
		{
			DomainEventHandler<SolutionRenamed> handler = Renaming;
			if (handler != null) handler(this, new DomainEventEventArgs<SolutionRenamed>(
				new SolutionRenamed(Id) { OldName = oldName, NewName = newName },
				doRename));
		}
	}
}