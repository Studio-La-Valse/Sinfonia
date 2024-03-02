namespace Sinfonia.Interfaces
{
    public interface IScoreBuilderFactory
    {
        (IScoreBuilder builder, IScoreDocumentReader document, IScoreLayoutProvider layout) Create(ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged);
    }
}
