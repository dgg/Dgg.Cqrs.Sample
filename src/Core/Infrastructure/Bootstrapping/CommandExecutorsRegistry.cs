using System;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Bootstrapping
{
	public class CommandExecutorsRegistry : Registry
	{
		public CommandExecutorsRegistry()
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
		public CommandExecutorsRegistry(Type t)
		{
			Scan(scanner =>
			{
				scanner.AssemblyContainingType(t);
				doScan(scanner);
			});
		}

		private static void doScan(IAssemblyScanner scanner)
		{
			scanner.ConnectImplementationsToTypesClosing(typeof(CommandExecutor<>));
		}
	}
}
