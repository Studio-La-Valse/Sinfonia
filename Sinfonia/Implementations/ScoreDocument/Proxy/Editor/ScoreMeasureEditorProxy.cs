using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class ScoreMeasureEditorProxy : IScoreMeasureEditor
    {
        private readonly ScoreMeasure source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;

        public int IndexInScore => source.IndexInScore;

        public Guid Guid => source.Guid;

        public TimeSignature TimeSignature => source.TimeSignature;

        public bool IsLastInScore => source.IsLastInScore;

        public int Id => source.Id;

        public KeySignature KeySignature => source.KeySignature;



        public ScoreMeasureEditorProxy(ScoreMeasure source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }




        public void EditKeySignature(KeySignature keySignature)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<ScoreMeasure, ScoreMeasureMemento>(source, s => s.EditKeySignature(keySignature)).ThenRerender(notifyEntityChanged, this);
            transaction.Enqueue(command);
        }

        public IInstrumentMeasureEditor ReadMeasure(int ribbonIndex)
        {
            return source.GetMeasureCore(ribbonIndex).ProxyEditor(commandManager, notifyEntityChanged);
        }

        public IEnumerable<IInstrumentMeasureEditor> ReadMeasures()
        {
            return source.EnumerateMeasuresCore().Select(e => e.ProxyEditor(commandManager, notifyEntityChanged));
        }

        public bool TryReadNext([NotNullWhen(true)] out IScoreMeasureEditor? next)
        {
            source.TryReadNext(out var _next);
            next = _next?.ProxyEditor(commandManager, notifyEntityChanged);
            return next != null;
        }

        public bool TryReadPrevious([NotNullWhen(true)] out IScoreMeasureEditor? previous)
        {
            source.TryReadPrevious(out var _previous);
            previous = _previous?.ProxyEditor(commandManager, notifyEntityChanged);
            return previous != null;
        }

        public IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return source.EnumerateChildren();
        }

        public bool Equals(IUniqueScoreElement? other)
        {
            return source.Equals(other);
        }
    }
}
