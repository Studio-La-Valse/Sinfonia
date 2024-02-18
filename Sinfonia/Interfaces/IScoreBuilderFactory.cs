namespace Sinfonia.Interfaces
{
    public interface IScoreBuilderFactory
    {
        (IScoreBuilder builder, IScoreDocumentReader document) Create(ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged);
    }
}
