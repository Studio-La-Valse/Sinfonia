namespace Sinfonia.Implementations.ScoreDocument;

public class TemplatePropertyWithInvalidator<T> : TemplateProperty<T>
{
    private readonly TemplateProperty<T> original;
    private readonly INotifyEntityChanged<IUniqueScoreElement> commandManager;
    private readonly IUniqueScoreElement toInvalidate;

    public TemplatePropertyWithInvalidator(TemplateProperty<T> original, INotifyEntityChanged<IUniqueScoreElement> commandManager, IUniqueScoreElement toInvalidate)
    {
        this.original = original;
        this.commandManager = commandManager;
        this.toInvalidate = toInvalidate;
    }

    public override T Value
    {
        get => original.Value;
        set
        {
            original.Value = value;
            commandManager.Invalidate(toInvalidate);
        }
    }

    public override void Reset()
    {
        original.Reset();
        commandManager.Invalidate(toInvalidate);
    }
}
