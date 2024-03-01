namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class InstrumentRibbonReaderProxy : IInstrumentRibbonReader
    {
        private readonly InstrumentRibbon source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;




        public int Id => source.Id;
        public Guid Guid => source.Guid;
        public int IndexInScore => source.IndexInScore;
        public Instrument Instrument => source.Instrument;




        public InstrumentRibbonReaderProxy(InstrumentRibbon source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }



        public IInstrumentMeasureReader ReadMeasure(int measureIndex)
        {
            return source.GetMeasureCore(measureIndex).Proxy(commandManager, notifyEntityChanged);
        }

        public IEnumerable<IInstrumentMeasureReader> ReadMeasures()
        {
            return source.EnumerateMeasuresCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
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
