using IScoreLayoutDictionary = StudioLaValse.ScoreDocument.Builder.IScoreLayoutDictionary;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class ScoreBuilder : IScoreBuilder
    {
        private readonly IScoreDocumentEditor scoreDocument;
        private readonly IScoreLayoutDictionary layoutProvider;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;
        private readonly Queue<Action<IScoreDocumentEditor, IScoreLayoutDictionary>> pendingEdits = [];

        public ScoreBuilder(IScoreDocumentEditor scoreDocument, IScoreLayoutDictionary layoutProvider, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.scoreDocument = scoreDocument;
            this.layoutProvider = layoutProvider;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }

        public IScoreBuilder Edit(Action<IScoreDocumentEditor> action)
        {
            pendingEdits.Enqueue((e, l) =>
            {
                action(e);
            });

            return this;
        }

        public IScoreBuilder Edit(Action<IScoreDocumentEditor, IScoreLayoutDictionary> action)
        {
            pendingEdits.Enqueue(action);
            return this;
        }

        public IScoreBuilder Build()
        {
            while (pendingEdits.Count > 0)
            {
                var pendingAction = pendingEdits.Dequeue();
                using var transaction = commandManager.OpenTransaction("Generic score document edit");
                pendingAction.Invoke(scoreDocument, layoutProvider);
            }

            notifyEntityChanged.RenderChanges();

            return this;
        }
    }
}
