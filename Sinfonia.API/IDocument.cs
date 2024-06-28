using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Key;
using StudioLaValse.ScoreDocument;

namespace Sinfonia.API
{
    public interface IDocument
    {
        IScoreBuilder ScoreBuilder { get; }
        ISelection<IUniqueScoreElement> Selection { get; }
        IDocumentUI DocumentUI { get; }
        IKeyGenerator<int> KeyGenerator { get; }
    }
}
