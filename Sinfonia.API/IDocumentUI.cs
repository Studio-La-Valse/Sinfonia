using StudioLaValse.Drawable;
using StudioLaValse.Drawable.Private;
using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Primitives;

namespace Sinfonia.API
{
    public interface IDocumentUI
    {
        void RebuildScene();
        INotifyEntityChanged<IUniqueScoreElement> EntityInvalidator { get; }
    }
}
