using System;
using System.Collections.Generic;
using System.Linq;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Data
{
	public interface IEventSession : IDisposable
	{
		void Save<T>(T item) where T : DomainEvent;
		void CommitChanges();
		void RollbackChanges();
		IEnumerable<T> For<T>(Guid receiverId) where T : DomainEvent;
		IEnumerable<T> For<T>(Guid receiverId, DateTimeOffset since) where T : DomainEvent;
		IQueryable<T> All<T>() where T : DomainEvent;
	}
}