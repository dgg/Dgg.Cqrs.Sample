using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Data
{
	internal static class QuerySessionExtensions
	{
		internal static T Single<T>(this IDocumentSession session, string id)
		{
			return session.Load<T>(id);
		}

		internal static T Single<T>(this IDocumentSession session, Guid id)
		{
			return Single<T>(session, id.ToString());
		}

		internal static IEnumerable<T> FindAll<T>(this IDocumentSession session)
		{
			return session.Query<T>().ToArray();
		}
	}
}
