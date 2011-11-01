using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Data
{
	public interface ISnapshotSession : IDisposable
	{
		IQueryable<T> All<T>() where T : class;
		T Single<T>(Expression<Func<T, bool>> expression) where T : class;
		void Save<T>(T item) where T : class;
		void Save<T>(IEnumerable<T> items) where T : class;
		void Delete<T>(T item) where T : class;
		void Delete<T>(Expression<Func<T, bool>> expression) where T : class;
		void DeleteAll<T>() where T : class;
		void CommitChanges();
		void RollbackChanges();
	}
}