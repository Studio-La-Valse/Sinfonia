namespace Sinfonia.ViewModels.Application.Document.Inspector
{
    public abstract class ScoreElementPropertiesViewModel : PropertyManagerViewModel
    {
        public IEnumerable<IUniqueScoreElement> Elements { get; }

        internal ScoreElementPropertiesViewModel(IEnumerable<IUniqueScoreElement> uniqueScoreElements)
        {
            Elements = uniqueScoreElements;
        }

        public abstract void Rebuild();
    }
}
