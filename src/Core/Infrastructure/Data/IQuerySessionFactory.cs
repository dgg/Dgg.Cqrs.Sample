using Raven.Client;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Data
{
	public interface IQuerySessionFactory
	{
		IDocumentSession CreateSession();
	}
}