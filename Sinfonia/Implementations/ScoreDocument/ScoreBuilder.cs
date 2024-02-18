using Sinfonia.Implementations.ScoreDocument.Proxy;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class ScoreBuilder : IScoreBuilder
    {
        private readonly ScoreDocumentProxy scoreDocument;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;
        private readonly Queue<Action<IScoreDocumentEditor>> pendingEdits = [];

        public IScoreDocumentReader ScoreDocument => scoreDocument;


        public ScoreBuilder(ScoreDocumentProxy scoreDocument, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.scoreDocument = scoreDocument;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }




        public IScoreBuilder Edit(Action<IScoreDocumentEditor> editor)
        {
            pendingEdits.Enqueue(editor);
            return this;
        }

        public IScoreBuilder Build()
        {
            while(pendingEdits.Count > 0)
            {
                var pendingAction = pendingEdits.Dequeue();
                using var transaction = commandManager.OpenTransaction("Generic score document edit");
                pendingAction.Invoke(scoreDocument);
            }

            notifyEntityChanged.RenderChanges();

            return this;
        }
    }
}
