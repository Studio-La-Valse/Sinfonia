namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class ScoreDocumentReaderProxy : IScoreDocumentReader
    {
        private readonly ScoreDocumentCore source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;

        public int NumberOfMeasures => source.NumberOfMeasures;
        public int NumberOfInstruments => source.NumberOfInstruments;
        public int Id => source.Id;
        public Guid Guid => source.Guid;



        public ScoreDocumentReaderProxy(ScoreDocumentCore score, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            source = score;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
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

        public IInstrumentRibbonReader ReadInstrumentRibbon(int indexInScore)
        {
            return source.GetInstrumentRibbonCore(indexInScore).Proxy(commandManager, notifyEntityChanged);
        }

        public IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return source.EnumerateChildren();
        }

        public IScoreMeasureReader ReadScoreMeasure(int indexInScore)
        {
            return source.GetScoreMeasureCore(indexInScore).Proxy(commandManager, notifyEntityChanged);
        }
    }
}
