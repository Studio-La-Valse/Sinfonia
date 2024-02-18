namespace Sinfonia.Implementations.ScoreDocument.Proxy
{
    internal class InstrumentRibbonProxy : IInstrumentRibbonEditor, IInstrumentRibbonReader
    {
        private readonly InstrumentRibbon source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;




        public int Id => source.Id;

        public int IndexInScore => source.IndexInScore;

        public string AbbreviatedName
        {
            get => source.AbbreviatedName;
            set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<InstrumentRibbon, InstrumentRibbonMemento>(source, s => s.AbbreviatedName = value);
                transaction.Enqueue(command);
            }
        }
        public bool Collapsed
        {
            get => source.Collapsed;
            set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<InstrumentRibbon, InstrumentRibbonMemento>(source, s => s.Collapsed = value);
                transaction.Enqueue(command);
            }
        }
        public string DisplayName
        {
            get => source.DisplayName;
            set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<InstrumentRibbon, InstrumentRibbonMemento>(source, s => s.DisplayName = value);
                transaction.Enqueue(command);
            }
        }
        public int NumberOfStaves
        {
            get => source.NumberOfStaves;
            set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<InstrumentRibbon, InstrumentRibbonMemento>(source, s => s.NumberOfStaves = value);
                transaction.Enqueue(command);
            }
        }



        public InstrumentRibbonProxy(InstrumentRibbon source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }



        public Instrument Instrument => source.Instrument;

        public IInstrumentMeasureEditor EditMeasure(int measureIndex)
        {
            return source.GetMeasureCore(measureIndex).Proxy(commandManager, notifyEntityChanged);
        }

        public IEnumerable<IInstrumentMeasureEditor> EditMeasures()
        {
            return source.EnumerateMeasuresCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return source.EnumerateChildren();
        }

        public IEnumerable<IInstrumentMeasure> EnumerateMeasures()
        {
            return source.EnumerateMeasuresCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public bool Equals(IUniqueScoreElement? other)
        {
            return source.Equals(other);
        }

        public IInstrumentMeasureReader ReadMeasure(int indexInScore)
        {
            return source.GetMeasureCore(indexInScore).Proxy(commandManager, notifyEntityChanged);
        }

        public IEnumerable<IInstrumentMeasureReader> ReadMeasures()
        {
            return source.EnumerateMeasuresCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }
    }
}
