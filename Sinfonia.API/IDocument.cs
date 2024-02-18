namespace Sinfonia.API
{
    public interface IDocument
    {
        IScoreBuilder ScoreBuilder { get; }
        ISelection<IUniqueScoreElement> Selection { get; }
        IDocumentUI DocumentUI { get; }
    }
}
