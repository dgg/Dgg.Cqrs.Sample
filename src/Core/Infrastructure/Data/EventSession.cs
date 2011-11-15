using System;
using System.Collections.Generic;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Data
{
	public class EventSession : IEventSession
	{
		private readonly IObjectContainer _db;
		[Obsolete("ioc")]
		public EventSession(){ }

		public EventSession(IObjectContainer container)
		{
			_db = container;
		}

		public void Save<T>(T item) where T : DomainEvent
		{
			_db.Store(item);
		}

		public IEnumerable<T> For<T>(Guid receiverId) where T : DomainEvent
		{
			return _db.Cast<T>().Where(e => e.ReceiverId.Equals(receiverId));
		}

		public IEnumerable<T> For<T>(Guid receiverId, DateTimeOffset since) where T : DomainEvent
		{
			return _db.Cast<T>().Where(e => e.ReceiverId.Equals(receiverId) && e.TimeStamp >= since);
		}

		public IQueryable<T> All<T>() where T : DomainEvent
		{
			return _db.Cast<T>().AsQueryable();
		}
		
		public void RollbackChanges()
		{
			_db.Rollback();
		}

		public void CommitChanges()
		{
			//commit the changes
			_db.Commit();
		}

		public void Dispose()
		{
			//explicitly close
			_db.Close();
			//dispose 'em
			_db.Dispose();
		}

	}
}
