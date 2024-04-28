namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal static class ScoreEditorExtensions
    {
        public static PageEditorProxy ProxyEditor(this Page page, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new PageEditorProxy(page, commandManager, notifyEntityChanged);
        }



        public static InstrumentRibbonEditorProxy ProxyEditor(this InstrumentRibbon instrumentRibbon, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new InstrumentRibbonEditorProxy(instrumentRibbon, commandManager, notifyEntityChanged);
        }

        public static ScoreMeasureEditorProxy ProxyEditor(this ScoreMeasure measureEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new ScoreMeasureEditorProxy(measureEditor, commandManager, notifyEntityChanged);
        }

        public static InstrumentMeasureEditorProxy ProxyEditor(this InstrumentMeasure measureEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new InstrumentMeasureEditorProxy(measureEditor, commandManager, notifyEntityChanged);
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






        public static StaffSystemEditorProxy ProxyEditor(this StaffSystem staff, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new StaffSystemEditorProxy(staff, commandManager, notifyEntityChanged);
        }

        public static StaffGroupEditorProxy ProxyEditor(this StaffGroup staff, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new StaffGroupEditorProxy(staff, commandManager, notifyEntityChanged);
        }

        public static StaffEditorProxy ProxyEditor(this Staff staff)
        {
            return new StaffEditorProxy(staff);
        }
    }
}
