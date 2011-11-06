<Query Kind="Program">
  <Reference Relative="..\src\Core\bin\Debug\Db4objects.Db4o.dll">D:\projects\Dgg.Cqrs.Sample\src\Core\bin\Debug\Db4objects.Db4o.dll</Reference>
  <Reference Relative="..\src\Core\bin\Debug\Db4objects.Db4o.Linq.dll">D:\projects\Dgg.Cqrs.Sample\src\Core\bin\Debug\Db4objects.Db4o.Linq.dll</Reference>
  <Reference Relative="..\src\Core\bin\Debug\Dgg.Cqrs.Sample.Core.dll">D:\projects\Dgg.Cqrs.Sample\src\Core\bin\Debug\Dgg.Cqrs.Sample.Core.dll</Reference>
  <Namespace>Db4objects.Db4o.Linq</Namespace>
  <Namespace>Db4objects.Db4o</Namespace>
  <Namespace>Dgg.Cqrs.Sample.Core.Domain.DefectHandling</Namespace>
</Query>

string snapshotsConnection = @"D:\Projects\Dgg.Cqrs.Sample\src\Web\App_Data\S_for_CQRS.db4o";

void Main()
{
	//dropSnapshots();
	dumpSnapshots();
}

void dumpSnapshots()
{
	using(var db = Db4oEmbedded.OpenFile(snapshotsConnection)){
		db.Cast<Solution>().AsQueryable().Dump();
		db.Cast<AppVersion>().AsQueryable().Dump();
		/*db.Cast<Build>().AsQueryable().Dump();
		db.Cast<OpenIssue>().AsQueryable().Dump();
		db.Cast<FixedIssue>().AsQueryable().Dump();
		db.Cast<ClosedIssue>().AsQueryable().Dump();*/
	}	
}

void dropSnapshots()
{
	using(var db = Db4oEmbedded.OpenFile(snapshotsConnection)){
		foreach (var s in db.Cast<Solution>()) db.Delete(s);
		foreach (var v in db.Cast<AppVersion>().AsQueryable())db.Delete(v);
		/*foreach (var v in db.Cast<Build>().AsQueryable())db.Delete(v);
		foreach (var v in db.Cast<Issue>().AsQueryable())db.Delete(v);*/
		db.Commit();
	}
}
// Define other methods and classes here