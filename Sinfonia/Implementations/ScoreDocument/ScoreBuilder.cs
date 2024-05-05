using Sinfonia.Implementations.ScoreDocument.Proxy.Editor;
using Sinfonia.Implementations.ScoreDocument.Proxy.Reader;
using StudioLaValse.ScoreDocument.Reader;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class ScoreBuilder : IScoreBuilder
    {
        private readonly ScoreDocumentCore scoreDocument;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;
        private readonly Queue<Action<IScoreDocumentEditor>> pendingEdits = [];

        public ScoreBuilder(ScoreDocumentCore scoreDocument, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.scoreDocument = scoreDocument;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }

        public IScoreBuilder Edit(Action<IScoreDocumentEditor> action)
        {
            pendingEdits.Enqueue(action);
            return this;
        }

        public IScoreBuilder Edit<TElement>(IEnumerable<int> elementIds, Action<TElement> action) where TElement : IScoreElementEditor
        {
            void _action(IScoreDocumentEditor editor)
            {
                var children = ((IScoreElement)editor).SelectRecursive(e => e.EnumerateChildren())
                    .OfType<IUniqueScoreElement>()
                    .Distinct(new KeyEqualityComparer<IUniqueScoreElement, int>(e => e.Id))
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


        public IScoreDocumentReader Build()
        {
            while (pendingEdits.Count > 0)
            {
                var pendingAction = pendingEdits.Dequeue();
                using var transaction = commandManager.OpenTransaction("Generic score document edit");
                pendingAction.Invoke(scoreDocument.ProxyEditor(commandManager, notifyEntityChanged));
            }

            notifyEntityChanged.RenderChanges();

            return scoreDocument.ProxyReader();
        }
    }
}
