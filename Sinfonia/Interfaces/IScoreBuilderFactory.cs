using Sinfonia.Implementations.ScoreDocument;
using IScoreLayoutDictionary = StudioLaValse.ScoreDocument.Layout.IScoreLayoutDictionary;

namespace Sinfonia.Interfaces
{
    public interface IScoreBuilderFactory
    {
        (IScoreBuilder builder, IScoreDocumentReader document, IScoreLayoutDictionary layout) Create(ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged);
    }
}
