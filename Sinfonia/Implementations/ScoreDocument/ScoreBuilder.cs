namespace Sinfonia.Implementations.ScoreDocument;

internal class ScoreBuilder : IScoreBuilder
{
    private readonly IScoreDocument scoreDocument;
    private readonly ICommandManager commandManager;
    private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;
    private readonly Queue<Action<IScoreDocument>> pendingEdits = [];
    private readonly IEqualityComparer<IUniqueScoreElement> equalityComparer = new KeyEqualityComparer<IUniqueScoreElement, int>(e => e.Id);

    public ScoreBuilder(IScoreDocument scoreDocument, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
    {
        this.scoreDocument = scoreDocument;
        this.commandManager = commandManager;
        this.notifyEntityChanged = notifyEntityChanged;
    }

    public IScoreBuilder Edit(Action<IScoreDocument> action)
    {
        pendingEdits.Enqueue(action);
        return this;
    }

    public IScoreBuilder Edit<TElement>(IEnumerable<int> elementIds, Action<TElement> action) where TElement : IUniqueScoreElement
    {
        void _action(IScoreDocument editor)
        {
            var children = ((IScoreElement)editor).SelectRecursive(e => e.EnumerateChildren())
                .OfType<IUniqueScoreElement>()
                .Distinct(equalityComparer)
                .Where(e => elementIds.Contains(e.Id))
                .OfType<TElement>();

            foreach (var child in children)
            {
                action(child);
            }
        }
        pendingEdits.Enqueue(_action);
        return this;
    }


    public IScoreBuilder Build()
    {
        while (pendingEdits.Count > 0)
        {
            var pendingAction = pendingEdits.Dequeue();
            using var transaction = commandManager.OpenTransaction("Generic score document edit");
            pendingAction.Invoke(scoreDocument);
        }

        notifyEntityChanged.RenderChanges();

        return this;
    }
}