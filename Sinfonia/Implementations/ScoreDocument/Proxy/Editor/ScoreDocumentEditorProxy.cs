namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class ScoreDocumentEditorProxy : IScoreDocumentEditor
    {
        private readonly ScoreDocumentCore source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;

        public int NumberOfMeasures => source.NumberOfMeasures;
        public int NumberOfInstruments => source.NumberOfInstruments;
        public int Id => source.Id;
        public Guid Guid => source.Guid;



        public ScoreDocumentEditorProxy(ScoreDocumentCore score, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = score;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }



        public void AddInstrumentRibbon(Instrument instrument)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentMemento>(source, s => s.AddInstrumentRibbon(instrument));
            transaction.Enqueue(command);
        }

        public void RemoveInstrumentRibbon(int indexInScore)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentMemento>(source, s => s.RemoveInstrumentRibbon(indexInScore));
            transaction.Enqueue(command);
        }


        public void AppendScoreMeasure(TimeSignature? timeSignature = null)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentMemento>(source, s => s.AppendScoreMeasure(timeSignature));
            transaction.Enqueue(command);
        }

        public void InsertScoreMeasure(int index, TimeSignature? timeSignature = null)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentMemento>(source, s => s.InsertScoreMeasure(index, timeSignature));
            transaction.Enqueue(command);
        }

        public void RemoveScoreMeasure(int index)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentMemento>(source, s => s.RemoveScoreMeasure(index));
            transaction.Enqueue(command);
        }

        public void Clear()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentMemento>(source, s => s.Clear());
            transaction.Enqueue(command);
        }





        public bool Equals(IUniqueScoreElement? other)
        {
            return source.Equals(other);
        }

        public IEnumerable<IScoreMeasureEditor> ReadScoreMeasures()
        {
            return source.EnumerateMeasuresCore().Select(e => e.ProxyEditor(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IInstrumentRibbonEditor> ReadInstrumentRibbons()
        {
            return source.EnumerateRibbonsCore().Select(e => e.ProxyEditor(commandManager, notifyEntityChanged));
        }

        public IInstrumentRibbonEditor ReadInstrumentRibbon(int indexInScore)
        {
            return source.GetInstrumentRibbonCore(indexInScore).ProxyEditor(commandManager, notifyEntityChanged);
        }

        public IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return source.EnumerateChildren();
        }

        public IScoreMeasureEditor ReadScoreMeasure(int indexInScore)
        {
            return source.GetScoreMeasureCore(indexInScore).ProxyEditor(commandManager, notifyEntityChanged);
        }
    }
}
