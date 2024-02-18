namespace Sinfonia.Implementations.ScoreDocument.Proxy
{
    internal static class ScoreEditorExtensions
    {
        public static ScoreDocumentProxy Proxy(this ScoreDocumentCore editor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new ScoreDocumentProxy(editor, commandManager, notifyEntityChanged);
        }




        public static InstrumentRibbonProxy Proxy(this InstrumentRibbon instrumentRibbon, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new InstrumentRibbonProxy(instrumentRibbon, commandManager, notifyEntityChanged);
        }

        public static ScoreMeasureProxy Proxy(this ScoreMeasure measureEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new ScoreMeasureProxy(measureEditor, commandManager, notifyEntityChanged);
        }

        public static InstrumentMeasureProxy Proxy(this InstrumentMeasure measureEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new InstrumentMeasureProxy(measureEditor, commandManager, notifyEntityChanged);
        }

        public static MeasureBlockChainProxy Proxy(this MeasureBlockChain measureEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new MeasureBlockChainProxy(measureEditor, commandManager, notifyEntityChanged);
        }

        public static MeasureBlockEditorProxy Proxy(this MeasureBlock chordGroup, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new MeasureBlockEditorProxy(chordGroup, commandManager, notifyEntityChanged);
        }

        public static ChordProxy Proxy(this Chord chordEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new ChordProxy(chordEditor, commandManager, notifyEntityChanged);
        }

        public static NoteProxy Proxy(this Note noteEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new NoteProxy(noteEditor, commandManager, notifyEntityChanged);
        }

        public static StaffProxy Proxy(this Staff staffEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new StaffProxy(staffEditor, commandManager, notifyEntityChanged);
        }

        public static StaffGroupProxy Proxy(this StaffGroup staffGroup, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new StaffGroupProxy(staffGroup, commandManager, notifyEntityChanged);
        }

        public static StaffSystemProxy Proxy(this StaffSystem staffSystem, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new StaffSystemProxy(staffSystem, commandManager, notifyEntityChanged);
        }
    }
}
