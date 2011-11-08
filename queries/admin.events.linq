<Query Kind="Program">
  <Reference Relative="..\src\Core\bin\Debug\Db4objects.Db4o.dll">D:\projects\Dgg.Cqrs.Sample\src\Core\bin\Debug\Db4objects.Db4o.dll</Reference>
  <Reference Relative="..\src\Core\bin\Debug\Db4objects.Db4o.Linq.dll">D:\projects\Dgg.Cqrs.Sample\src\Core\bin\Debug\Db4objects.Db4o.Linq.dll</Reference>
  <Reference Relative="..\src\Core\bin\Debug\Dgg.Cqrs.Sample.Core.dll">D:\projects\Dgg.Cqrs.Sample\src\Core\bin\Debug\Dgg.Cqrs.Sample.Core.dll</Reference>
  <Namespace>Db4objects.Db4o.Linq</Namespace>
  <Namespace>Db4objects.Db4o</Namespace>
  <Namespace>Dgg.Cqrs.Sample.Core.Domain.Admin.Events</Namespace>
</Query>

string eventsConnection = @"D:\Projects\Dgg.Cqrs.Sample\src\Web\App_Data\E_for_CQRS.db4o";

void Main()
{
	//dropEvents();
	dumpEvents();
}

void dumpEvents()
{
	using(var db = Db4oEmbedded.OpenFile(eventsConnection)){
		db.Cast<Dgg.Cqrs.Sample.Core.Infrastructure.Eventing.DomainEvent>()
			.AsQueryable()
			.OrderBy(e => e.TimeStamp)
			.ThenBy(e => e.EventId)
			.Select(e =>
			new{
				id = e.EventId,
				what = e.GetType().Name,
				who = e.ReceiverId,
				when = e.TimeStamp
			}).Dump();
	}
}

void dropEvents()
{
	using(var db = Db4oEmbedded.OpenFile(eventsConnection)){
		foreach(var e in db.Cast<Dgg.Cqrs.Sample.Core.Infrastructure.Eventing.DomainEvent>()) db.Delete(e);
		db.Commit();
	}
}
// Define other methods and classes here