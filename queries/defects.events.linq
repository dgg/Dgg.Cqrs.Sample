<Query Kind="Program">
  <Reference Relative="..\src\Core\bin\Debug\Mono.Cecil.dll">D:\Projects\Dgg.Anug.Cqrs\trunk\src\Core\bin\Debug\Mono.Cecil.dll</Reference>
  <Reference Relative="..\src\Core\bin\Debug\Db4objects.Db4o.dll">D:\Projects\Dgg.Anug.Cqrs\trunk\src\Core\bin\Debug\Db4objects.Db4o.dll</Reference>
  <Reference Relative="..\src\Core\bin\Debug\Cecil.FlowAnalysis.dll">D:\Projects\Dgg.Anug.Cqrs\trunk\src\Core\bin\Debug\Cecil.FlowAnalysis.dll</Reference>
  <Reference Relative="..\src\Core\bin\Debug\Db4objects.Db4o.Linq.dll">D:\Projects\Dgg.Anug.Cqrs\trunk\src\Core\bin\Debug\Db4objects.Db4o.Linq.dll</Reference>
  <Reference Relative="..\lib\RavenDb\Client\Raven.Client.Lightweight.dll">D:\Projects\Dgg.Anug.Cqrs\trunk\lib\RavenDb\Client\Raven.Client.Lightweight.dll</Reference>
  <Reference Relative="..\lib\RavenDb\Client\Newtonsoft.Json.dll">D:\Projects\Dgg.Anug.Cqrs\trunk\lib\RavenDb\Client\Newtonsoft.Json.dll</Reference>
  <Reference Relative="..\src\Core\bin\Debug\Dgg.Anug.Cqrs.Core.dll">D:\Projects\Dgg.Anug.Cqrs\trunk\src\Core\bin\Debug\Dgg.Anug.Cqrs.Core.dll</Reference>
  <Namespace>Db4objects.Db4o.Linq</Namespace>
  <Namespace>Db4objects.Db4o</Namespace>
  <Namespace>Dgg.Anug.Cqrs.Core.Domain.DefectHandling.Events</Namespace>
  <Namespace>Dgg.Anug.Cqrs.Core.Domain.DefectHandling</Namespace>
</Query>

string eventsConnection = @"D:\Projects\Dgg.Anug.Cqrs\trunk\tools\db4o\App_Data\E_for_CQRS.db4o";
string snapshotsConnection = @"D:\Projects\Dgg.Anug.Cqrs\trunk\tools\db4o\App_Data\S_for_CQRS.db4o";

void Main()
{
	//dropEvents();
	//dropSnapshots();
	dumpEvents();
	dumpSnapshots();
}

void dumpSnapshots()
{
	using(var db = Db4oEmbedded.OpenFile(snapshotsConnection)){
		db.Cast<Solution>().AsQueryable().Dump();
		db.Cast<AppVersion>().AsQueryable().Dump();
		db.Cast<Build>().AsQueryable().Dump();
		db.Cast<OpenIssue>().AsQueryable().Dump();
		db.Cast<FixedIssue>().AsQueryable().Dump();
		db.Cast<ClosedIssue>().AsQueryable().Dump();
	}	
}

void dropSnapshots()
{
	using(var db = Db4oEmbedded.OpenFile(snapshotsConnection)){
		foreach (var s in db.Cast<Solution>()) db.Delete(s);
		foreach (var v in db.Cast<AppVersion>().AsQueryable())db.Delete(v);
		foreach (var v in db.Cast<Build>().AsQueryable())db.Delete(v);
		foreach (var v in db.Cast<Issue>().AsQueryable())db.Delete(v);
		db.Commit();
	}
}

void dumpEvents()
{
	using(var db = Db4oEmbedded.OpenFile(eventsConnection)){
		db.Cast<Dgg.Anug.Cqrs.Core.Infrastructure.Eventing.DomainEvent>()
			.AsQueryable()
			.Where(e => e.GetType().Name.Contains("Issue"))
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
		foreach(var e in db.Cast<Dgg.Anug.Cqrs.Core.Infrastructure.Eventing.DomainEvent>().Where(e => e.GetType().Name.Contains("Issue"))) db.Delete(e);
		db.Commit();
	}
}
// Define other methods and classes here