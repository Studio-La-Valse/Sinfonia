namespace Sinfonia.Implementations.ScoreDocument;

public static class TemplatePropertiesExensions
{
    public static TemplateProperty<T> UseCommandManager<T>(this TemplateProperty<T> templateProperty, ICommandManager commandManager)
    {
        return new TemplatePropertyWithCommandManager<T>(templateProperty, commandManager);
    }

    public static TemplateProperty<T> ThenInvalidate<T>(this TemplateProperty<T> templateProperty, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged,  IUniqueScoreElement toInvalidate)
    {
        return new TemplatePropertyWithInvalidator<T>(templateProperty, notifyEntityChanged, toInvalidate);
    }
}
