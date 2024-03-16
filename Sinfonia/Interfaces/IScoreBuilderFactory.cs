using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Interfaces
{
    public interface IScoreBuilderFactory
    {
        (IScoreBuilder builder, IScoreDocumentReader document, IScoreDocumentLayout layout) Create(ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged, ScoreDocumentStyleTemplate scoreDocumentStyleTemplate);
    }
}
