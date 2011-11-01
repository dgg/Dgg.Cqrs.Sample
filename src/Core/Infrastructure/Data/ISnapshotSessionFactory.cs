namespace Dgg.Cqrs.Sample.Core.Infrastructure.Data
{
	public interface ISnapshotSessionFactory
	{
		ISnapshotSession CreateSession();
	}
}