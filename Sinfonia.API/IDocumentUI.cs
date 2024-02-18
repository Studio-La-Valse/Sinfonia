namespace Sinfonia.API
{
    public interface IDocumentUI
    {
        void RebuildScene();
        INotifyEntityChanged<IUniqueScoreElement> EntityInvalidator { get; }
    }
}
