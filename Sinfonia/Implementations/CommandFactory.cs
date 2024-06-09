using ReactiveUI;

namespace Sinfonia.Implementations;

internal class CommandFactory : ICommandFactory
{
    public ICommand Create(Action action, Func<bool>? canExecute = null)
    {
        return ReactiveCommand.Create(action);
    }

    public ICommand Create<T>(Action<T> action, Func<T, bool>? canExecute = null)
    {
        return ReactiveCommand.Create(action);
    }
}
