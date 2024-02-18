namespace Sinfonia.Implementations.ScoreDocument.Proxy
{
    internal class ScoreDocumentProxy : IScoreDocumentEditor, IScoreDocumentReader
    {
        private readonly ScoreDocumentCore source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;

        public int NumberOfMeasures => source.NumberOfMeasures;
        public int NumberOfInstruments => source.NumberOfInstruments;
        public int Id => source.Id;

        public ScoreDocumentProxy(ScoreDocumentCore score, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            source = score;
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




        public IEnumerable<IInstrumentRibbonEditor> EditInstrumentRibbons()
        {
            return source.EnumerateRibbonsCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }
        public IEnumerable<IScoreMeasureEditor> EditScoreMeasures()
        {
            return source.EnumerateMeasuresCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }
        public IEnumerable<IStaffSystemEditor> EditStaffSystems()
        {
            return source.EnumerateStaffSystemsCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }




        public IScoreMeasureEditor EditScoreMeasure(int indexInScore)
        {
            return source.GetScoreMeasureCore(indexInScore).Proxy(commandManager, notifyEntityChanged);
        }
        public IInstrumentRibbonEditor EditInstrumentRibbon(int indexInScore)
        {
            return source.GetInstrumentRibbonCore(indexInScore).Proxy(commandManager, notifyEntityChanged);
        }





        public bool Equals(IUniqueScoreElement? other)
        {
            return source.Equals(other);
        }


        public IEnumerable<IScoreMeasureReader> ReadScoreMeasures()
        {
            return source.EnumerateMeasuresCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IInstrumentRibbonReader> ReadInstrumentRibbons()
        {
            return source.EnumerateRibbonsCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IStaffSystemReader> ReadStaffSystems()
        {
            return source.EnumerateStaffSystemsCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public IScoreMeasureReader ReadMeasure(int indexInScore)
        {
            return source.GetScoreMeasureCore(indexInScore).Proxy(commandManager, notifyEntityChanged);
        }

        public IInstrumentRibbonReader ReadInstrumentRibbon(int indexInScore)
        {
            return source.GetInstrumentRibbonCore(indexInScore).Proxy(commandManager, notifyEntityChanged);
        }

        public IEnumerable<IScoreMeasure> EnumerateScoreMeasures()
        {
            return source.EnumerateMeasuresCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IInstrumentRibbon> EnumerateInstrumentRibbons()
        {
            return source.EnumerateRibbonsCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return source.EnumerateChildren();
        }
    }
}
