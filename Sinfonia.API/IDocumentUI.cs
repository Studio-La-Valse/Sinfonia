using StudioLaValse.Drawable;
using StudioLaValse.ScoreDocument.Core.Primitives;

namespace Sinfonia.API
{
    public interface IDocumentUI
    {
        void RebuildScene();
        INotifyEntityChanged<IUniqueScoreElement> EntityInvalidator { get; }
    }
}
