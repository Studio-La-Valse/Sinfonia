namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal static class ScoreEditorExtensions
    {
        public static ScoreDocumentEditorProxy ProxyEditor(this ScoreDocumentCore editor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new ScoreDocumentEditorProxy(editor, commandManager, notifyEntityChanged);
        }




        public static InstrumentRibbonEditorProxy ProxyEditor(this InstrumentRibbon instrumentRibbon, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new InstrumentRibbonEditorProxy(instrumentRibbon, commandManager, notifyEntityChanged);
        }

        public static ScoreMeasureEditorProxy ProxyEditor(this ScoreMeasure measureEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new ScoreMeasureEditorProxy(measureEditor, commandManager, notifyEntityChanged);
        }

        public static InstrumentEditorMeasureProxy ProxyEditor(this InstrumentMeasure measureEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new InstrumentEditorMeasureProxy(measureEditor, commandManager, notifyEntityChanged);
        }

        public static MeasureBlockChainEditorProxy ProxyEditor(this MeasureBlockChain measureEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new MeasureBlockChainEditorProxy(measureEditor, commandManager, notifyEntityChanged);
        }

        public static MeasureBlockEditorProxy ProxyEditor(this MeasureBlock chordGroup, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new MeasureBlockEditorProxy(chordGroup, commandManager, notifyEntityChanged);
        }

        public static ChordEditorProxy ProxyEditor(this Chord chordEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new ChordEditorProxy(chordEditor, commandManager, notifyEntityChanged);
        }

        public static NoteEditorProxy ProxyEditor(this Note noteEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new NoteEditorProxy(noteEditor, commandManager, notifyEntityChanged);
        }
    }
}
