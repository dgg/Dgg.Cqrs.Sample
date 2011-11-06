using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Data
{
	public interface IEventSession : IDisposable
	{
		IQueryable<T> All<T>() where T : class;
		T Single<T>(Expression<Func<T, bool>> expression) where T : class;
		void Save<T>(T item) where T : class;
		void Save<T>(IEnumerable<T> items) where T : class;
		void CommitChanges();
		void RollbackChanges();
	}
}