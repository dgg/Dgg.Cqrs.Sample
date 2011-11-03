namespace Dgg.Cqrs.Sample.Core.Infrastructure.Commanding
{
	public abstract class CommandExecutor<TCommand> where TCommand : ICommand
	{
		public abstract void Execute(TCommand cmd);
	}
}
