using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Key;
using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Builder;
using StudioLaValse.ScoreDocument.Core.Primitives;

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
