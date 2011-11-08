using System;
using System.Collections.Generic;
using Dgg.Cqrs.Sample.Core.Domain.Admin.Events;
using Dgg.Cqrs.Sample.Core.Infrastructure;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;

namespace Dgg.Cqrs.Sample.Core.Domain.Admin
{
	public class Build
	{
		public Build(string name)
		{
			OnCreating(Comb.Generate(), name);
		}

		private bool doCreate(BuildCreated e)
		{
			Id = e.ReceiverId;
			Name = e.Name;
			return true;
		}

		public Build(Guid id, string name)
		{
			Id = id;
			Name = name;
		}

		public Guid Id { get; private set; }
		public Solution Solution { get; private set; }
		public string Name { get; private set; }

		public Build Rename(string newName)
		{
			OnRenaming(Name, newName);
			return this;
		}
		private bool doRename(BuildRenamed e)
		{
			Name = e.NewName;
			return true;
		}

		public Build Assign(Solution solution)
		{
			OnAssigning(Solution, solution);
			return this;
		}
		private bool doAssign(BuildAssigned e)
		{
			Solution = e.NewlyAssigned;
			return true;
		}

		public event DomainEventHandler<BuildRenamed> Renaming;
		protected virtual void OnRenaming(string oldName, string newName)
		{
			DomainEventHandler<BuildRenamed> handler = Renaming;
			if (handler != null) handler(this, new DomainEventEventArgs<BuildRenamed>(
				new BuildRenamed(Id) { OldName = oldName, NewName = newName },
				doRename));
		}

		/// <summary>
		/// STATIC, do unsubscribe from it
		/// </summary>
		public static event DomainEventHandler<BuildCreated> Creating;
		// not virtual as is called from the ctor
		protected void OnCreating(Guid id, string name)
		{
			DomainEventHandler<BuildCreated> handler = Creating;
			if (handler != null) handler(this, new DomainEventEventArgs<BuildCreated>(
				new BuildCreated(id) { Name = name },
				doCreate));
		}

		public event DomainEventHandler<BuildAssigned> Assigning;
		protected virtual void OnAssigning(Solution previouslyAssigned, Solution newlyAssigned)
		{
			DomainEventHandler<BuildAssigned> handler = Assigning;
			if (handler != null) handler(this, new DomainEventEventArgs<BuildAssigned>(
				new BuildAssigned(Id) { PreviouslyAssigned = previouslyAssigned, NewlyAssigned = newlyAssigned },
				doAssign));
		}

		public static IEnumerable<Build> CreateRange(string prefix, string suffix, int start, int end)
		{
			for (int i = start; i <= end; i++)
			{
				yield return new Build(buildName(prefix, suffix, i));
			}
		}

		private static string buildName(string pre, string post, int i)
		{
			return pre + i + post;
		}
	}
}
