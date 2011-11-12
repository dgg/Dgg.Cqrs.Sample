using Dgg.Cqrs.Sample.Core.Infrastructure.Bootstrapping;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using NUnit.Framework;
using StructureMap;

namespace Dgg.Cqrs.Sample.Core.Tests.Infrastructure.Bootstrapping
{
	[TestFixture]
	public class CommandExecutorRegistryTester
	{
		[Test]
		public void RegistersInstances_CanResolveExecutorForEachCommand()
		{
			IContainer container = new Container(new CommandExecutorsRegistry(typeof(CommandExecutorRegistryTester)));

			container.AssertConfigurationIsValid();
			Assert.That(container.GetInstance(typeof(CommandExecutor<Command1>)), Is.InstanceOf<Executor1>());
			Assert.That(container.GetInstance(typeof(CommandExecutor<Command2>)), Is.InstanceOf<Executor2>());
		}

		[Test]
		public void AllInstances_CanBeInjected()
		{
			IContainer container = new Container(new CommandExecutorsRegistry(typeof(CommandExecutorRegistryTester)));
			container.Configure(config => config.For<UsesExecutors>());

			var instance = container.GetInstance<UsesExecutors>();
			Assert.That(instance, Has.
				Property("Executor1").InstanceOf<Executor1>().And.
				Property("Executor2").InstanceOf<Executor2>());
		}
	}

	public class Command1 : ICommand { }
	public class Command2 : ICommand { }

	public class Executor1 : CommandExecutor<Command1> { public override void Execute(Command1 cmd) { } }
	public class Executor2 : CommandExecutor<Command2> { public override void Execute(Command2 cmd) { } }

	public class UsesExecutors
	{
		public UsesExecutors(CommandExecutor<Command1> executor1, CommandExecutor<Command2> executor2)
		{
			Executor1 = executor1;
			Executor2 = executor2;
		}

		public CommandExecutor<Command1> Executor1 { get; private set; }
		public CommandExecutor<Command2> Executor2 { get; private set; }
	}
}
