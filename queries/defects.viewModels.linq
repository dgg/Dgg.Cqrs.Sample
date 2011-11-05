<Query Kind="Program">
  <Reference Relative="..\..\..\Dgg.Cqrs.Sample\packages\Newtonsoft.Json.4.0.2\lib\net40\Newtonsoft.Json.dll">D:\projects\Dgg.Cqrs.Sample\packages\Newtonsoft.Json.4.0.2\lib\net40\Newtonsoft.Json.dll</Reference>
  <Reference Relative="..\..\..\Dgg.Cqrs.Sample\packages\NLog.2.0.0.2000\lib\net40\NLog.dll">D:\projects\Dgg.Cqrs.Sample\packages\NLog.2.0.0.2000\lib\net40\NLog.dll</Reference>
  <Reference Relative="..\..\..\Dgg.Cqrs.Sample\packages\RavenDB.1.0.499\lib\net40\Raven.Client.Lightweight.dll">D:\projects\Dgg.Cqrs.Sample\packages\RavenDB.1.0.499\lib\net40\Raven.Client.Lightweight.dll</Reference>
  <Reference Relative="..\..\..\Dgg.Cqrs.Sample\src\Core\bin\Debug\Dgg.Cqrs.Sample.Core.dll">D:\projects\Dgg.Cqrs.Sample\src\Core\bin\Debug\Dgg.Cqrs.Sample.Core.dll</Reference>
  <Namespace>Raven.Client</Namespace>
  <Namespace>Raven.Client.Document</Namespace>
  <Namespace>Dgg.Cqrs.Sample.Core.Presentation.Models.DefectHandling</Namespace>
</Query>

string viewModelsConnection = @"http://localhost:8080";

void Main()
{
	//dropViewModels();
	dumpViewModels();
}

// Define other methods and classes here
void dumpViewModels()
{
	using (DocumentStore store = new DocumentStore { Url = viewModelsConnection })
	{
		store.Initialize();
		using (var session = store.OpenSession())
		{
			session.Query<Issue>().Dump();
			session.Query<SupportEntities>().Dump();
		}
	}
}

void dropViewModels()
{
	using (DocumentStore store = new DocumentStore { Url = viewModelsConnection })
	{
		store.Initialize();
		using (var session = store.OpenSession())
		{
			Array.ForEach(session.Query<Issue>().ToArray(), s => session.Delete(s));
			Array.ForEach(session.Query<SupportEntities>().ToArray(), v => session.Delete(v));
			session.SaveChanges();
		}
	}
	
}