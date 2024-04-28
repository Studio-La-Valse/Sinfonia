namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal static class ScoreReaderExtensions
    {
        public static ScoreDocumentReaderProxy ProxyReader(this ScoreDocumentCore editor)
        {
            return new ScoreDocumentReaderProxy(editor);
        }
        public static PageReaderProxy Proxy(this Page page)
        {
            return new PageReaderProxy(page);
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



        public static StaffSystemReaderProxy Proxy(this StaffSystem staff)
        {
            return new StaffSystemReaderProxy(staff);
        }

        public static StaffGroupProxy Proxy(this StaffGroup staff)
        {
            return new StaffGroupProxy(staff);
        }

        public static StaffReaderProxy Proxy(this Staff staff)
        {
            return new StaffReaderProxy(staff);
        }
    }
}
