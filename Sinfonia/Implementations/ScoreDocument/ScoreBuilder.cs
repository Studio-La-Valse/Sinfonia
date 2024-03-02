using Sinfonia.Extensions;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class ScoreBuilder : IScoreBuilder
    {
        private readonly IScoreDocumentEditor scoreDocument;
        private readonly IScoreLayoutBuilder scoreLayoutDictionary;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;
        private readonly Queue<Action<IScoreDocumentEditor, IScoreLayoutBuilder>> pendingEdits = [];

        public ScoreBuilder(IScoreDocumentEditor scoreDocument, IScoreLayoutBuilder scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.scoreDocument = scoreDocument;
            this.scoreLayoutDictionary = scoreLayoutDictionary;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }

        public IScoreBuilder Edit(Action<IScoreDocumentEditor> action)
        {
            return Edit((e, l) => action(e));
        }

        public IScoreBuilder Edit(Action<IScoreDocumentEditor, IScoreLayoutBuilder> action)
        {
            pendingEdits.Enqueue(action);
            return this;
        }

        public IScoreBuilder Edit<TElement>(IEnumerable<int> elementIds, Action<TElement> action) where TElement : IScoreElementEditor
        {
            return Edit<TElement>(elementIds, (e, l) => action(e));
        }

        public IScoreBuilder Edit<TElement>(IEnumerable<int> elementIds, Action<TElement, IScoreLayoutBuilder> action) where TElement : IScoreElementEditor
        {
            void _action(IScoreDocumentEditor scoreBuilder, IScoreLayoutBuilder layoutDictionary)
            {
                var children = ((IScoreElement)scoreBuilder).SelectRecursive(e => e.EnumerateChildren())
                    .OfType<IUniqueScoreElement>()
                    .Distinct(new KeyEqualityComparer<IUniqueScoreElement, int>(e => e.Id))
                    .Where(e => elementIds.Contains(e.Id))
                    .OfType<TElement>();

                foreach (var child in children)
                {
                    action(child, layoutDictionary);
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
                pendingAction.Invoke(scoreDocument, scoreLayoutDictionary);
            }

            notifyEntityChanged.RenderChanges();

            return this;
        }

    }
}
