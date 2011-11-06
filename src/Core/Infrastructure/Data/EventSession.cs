using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

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

		public IQueryable<T> All<T>() where T : class
		{
			return _db.Cast<T>().Select(items => items).AsQueryable();
		}

		public T Single<T>(Expression<Func<T, bool>> expression) where T : class
		{
			return All<T>().SingleOrDefault(expression);
		}

		public void Save<T>(T item) where T : class
		{
			_db.Store(item);
		}

		public void Save<T>(IEnumerable<T> items) where T : class
		{
			foreach (var item in items)
			{
				_db.Store(item);
			}
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
