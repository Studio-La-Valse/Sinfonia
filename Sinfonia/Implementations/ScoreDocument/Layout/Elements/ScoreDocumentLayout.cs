namespace Sinfonia.Implementations.ScoreDocument.Layout.Elements
{
    /// <summary>
    /// The layout of a score document.
    /// </summary>
    public class ScoreDocumentLayout : IScoreDocumentLayout
    {
        /// <inheritdoc/>
        public string Title { get; }
        /// <inheritdoc/>
        public string SubTitle { get; }

        /// <summary>
        /// Create a new layout.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="subTitle"></param>
        public ScoreDocumentLayout(string title, string subTitle)
        {
            Title = title;
            SubTitle = subTitle;
        }

        /// <inheritdoc/>
        public IScoreDocumentLayout Copy()
        {
            return new ScoreDocumentLayout(Title, SubTitle);
        }
    }
}