using Sinfonia.Implementations.ScoreDocument.Proxy.Reader;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal static class ScoreReaderExtensions
    {
        public static ScoreDocumentReaderProxy Proxy(this ScoreDocumentCore editor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new ScoreDocumentReaderProxy(editor, commandManager, notifyEntityChanged);
        }




        public static InstrumentRibbonReaderProxy Proxy(this InstrumentRibbon instrumentRibbon, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new InstrumentRibbonReaderProxy(instrumentRibbon, commandManager, notifyEntityChanged);
        }

        public static ScoreMeasureReaderProxy Proxy(this ScoreMeasure measureEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new ScoreMeasureReaderProxy(measureEditor, commandManager, notifyEntityChanged);
        }

        public static InstrumentMeasureReaderProxy Proxy(this InstrumentMeasure measureEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new InstrumentMeasureReaderProxy(measureEditor, commandManager, notifyEntityChanged);
        }

        public static MeasureBlockChainReaderProxy Proxy(this MeasureBlockChain measureEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new MeasureBlockChainReaderProxy(measureEditor, commandManager, notifyEntityChanged);
        }

        public static MeasureBlockReaderProxy Proxy(this MeasureBlock chordGroup, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new MeasureBlockReaderProxy(chordGroup, commandManager, notifyEntityChanged);
        }

        public static ChordReaderProxy Proxy(this Chord chordEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new ChordReaderProxy(chordEditor, commandManager, notifyEntityChanged);
        }

        public static NoteReaderProxy Proxy(this Note noteEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new NoteReaderProxy(noteEditor, commandManager, notifyEntityChanged);
        }
    }
}
