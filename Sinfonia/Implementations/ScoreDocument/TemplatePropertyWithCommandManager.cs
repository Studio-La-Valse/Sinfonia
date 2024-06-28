namespace Sinfonia.Implementations.ScoreDocument;

public class TemplatePropertyWithCommandManager<T> : TemplateProperty<T>
{
    private readonly TemplateProperty<T> original;
    private readonly ICommandManager commandManager;

    public TemplatePropertyWithCommandManager(TemplateProperty<T> original, ICommandManager commandManager)
    {
        this.original = original;
        this.commandManager = commandManager;
    }

    public override T Value
    {
        get => original.Value;
        set
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var oldValue = original.Value;
            var command = new SimpleCommand(() => original.Value = value, () => original.Value = oldValue);
            transaction.Enqueue(command);
        }
    }

    public override void Reset()
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var oldValue = original.Value;
        var command = new SimpleCommand(original.Reset, () => original.Value = oldValue);
        transaction.Enqueue(command);
    }
}
