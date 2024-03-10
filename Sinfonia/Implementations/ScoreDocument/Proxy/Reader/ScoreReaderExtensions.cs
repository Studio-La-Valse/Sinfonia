namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal static class ScoreReaderExtensions
    {
        public static ScoreDocumentReaderProxy Proxy(this ScoreDocumentCore editor)
        {
            return new ScoreDocumentReaderProxy(editor);
        }
        public static PageReaderProxy Proxy(this Page page)
        {
            return new PageReaderProxy(page);
        }


        public static InstrumentRibbonReaderProxy Proxy(this InstrumentRibbon instrumentRibbon)
        {
            return new InstrumentRibbonReaderProxy(instrumentRibbon);
        }

        public static ScoreMeasureReaderProxy Proxy(this ScoreMeasure measureEditor)
        {
            return new ScoreMeasureReaderProxy(measureEditor);
        }

        public static InstrumentMeasureReaderProxy Proxy(this InstrumentMeasure measureEditor)
        {
            return new InstrumentMeasureReaderProxy(measureEditor);
        }



        public static MeasureBlockChainReaderProxy Proxy(this MeasureBlockChain measureEditor)
        {
            return new MeasureBlockChainReaderProxy(measureEditor);
        }

        public static MeasureBlockReaderProxy Proxy(this MeasureBlock chordGroup)
        {
            return new MeasureBlockReaderProxy(chordGroup);
        }

        public static ChordReaderProxy Proxy(this Chord chordEditor)
        {
            return new ChordReaderProxy(chordEditor);
        }

        public static NoteReaderProxy Proxy(this Note noteEditor)
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
