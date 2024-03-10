using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Builder;

namespace Sinfonia.API
{
    public interface IDocument
    {
        IScoreBuilder ScoreBuilder { get; }
        ISelection<IUniqueScoreElement> Selection { get; }
        IDocumentUI DocumentUI { get; }
    }
}
