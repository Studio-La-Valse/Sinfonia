namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal static class ScoreReaderExtensions
    {
        public static ScoreDocumentReaderProxy ProxyReader(this ScoreDocumentCore editor)
        {
            return new ScoreDocumentReaderProxy(editor);
        }


        public static InstrumentRibbonReaderProxy ProxyReader(this InstrumentRibbon instrumentRibbon)
        {
            return new InstrumentRibbonReaderProxy(instrumentRibbon);
        }

        public static ScoreMeasureReaderProxy ProxyReader(this ScoreMeasure measureEditor)
        {
            return new ScoreMeasureReaderProxy(measureEditor);
        }

        public static InstrumentMeasureReaderProxy ProxyReader(this InstrumentMeasure measureEditor)
        {
            return new InstrumentMeasureReaderProxy(measureEditor);
        }



        public static MeasureBlockChainReaderProxy Proxy(this MeasureBlockChain measureEditor)
        {
            return new MeasureBlockChainReaderProxy(measureEditor);
        }

        public static MeasureBlockReaderProxy ProxyReader(this MeasureBlock chordGroup)
        {
            return new MeasureBlockReaderProxy(chordGroup);
        }

        public static ChordReaderProxy ProxyReader(this Chord chordEditor)
        {
            return new ChordReaderProxy(chordEditor);
        }

        public static NoteReaderProxy ProxyReader(this Note noteEditor)
        {
            return new NoteReaderProxy(noteEditor);
        }
    }
}
