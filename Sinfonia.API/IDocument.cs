using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Key;
using StudioLaValse.ScoreDocument.Builder;
using StudioLaValse.ScoreDocument.Primitives;
using StudioLaValse.ScoreDocument.Reader;

namespace Sinfonia.API
{
    public interface IDocument
    {
        IScoreDocumentReader ScoreReader { get; }
        IScoreBuilder ScoreBuilder { get; }
        ISelection<IUniqueScoreElement> Selection { get; }
        IDocumentUI DocumentUI { get; }
        IKeyGenerator<int> KeyGenerator { get; }
    }
}
