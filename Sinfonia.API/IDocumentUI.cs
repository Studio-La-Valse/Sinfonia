using StudioLaValse.Drawable;
using StudioLaValse.ScoreDocument;

namespace Sinfonia.API
{
    public interface IDocumentUI
    {
        void RebuildScene();
        INotifyEntityChanged<IUniqueScoreElement> EntityInvalidator { get; }
    }
}
