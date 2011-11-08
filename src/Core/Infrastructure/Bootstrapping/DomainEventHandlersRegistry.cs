using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Eventing;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Bootstrapping
{
	public class DomainEventHandlersRegistry : Registry
	{
		public DomainEventHandlersRegistry()
		{
			Scan(scanner =>
			{
				scanner.TheCallingAssembly();
				doScan(scanner);
			});
		}

		/// <summary>
		/// Ctor for testing purposes
		/// </summary>
		public DomainEventHandlersRegistry(Type t)
		{
			Scan(scanner =>
			{
				scanner.AssemblyContainingType(t);
				doScan(scanner);
			});
		}

		private static void doScan(IAssemblyScanner scanner)
		{
			scanner.ConnectImplementationsToTypesClosing(typeof(IHandler<>));
		}
	}
}
