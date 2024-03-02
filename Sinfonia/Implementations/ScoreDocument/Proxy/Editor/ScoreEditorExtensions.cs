namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal static class ScoreEditorExtensions
    {
        public static ScoreDocumentEditorProxy ProxyEditor(this ScoreDocumentCore editor, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new ScoreDocumentEditorProxy(editor, scoreLayoutDictionary, commandManager, notifyEntityChanged);
        }




        public static InstrumentRibbonEditorProxy ProxyEditor(this InstrumentRibbon instrumentRibbon, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new InstrumentRibbonEditorProxy(instrumentRibbon, scoreLayoutDictionary, commandManager, notifyEntityChanged);
        }

        public static ScoreMeasureEditorProxy ProxyEditor(this ScoreMeasure measureEditor, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new ScoreMeasureEditorProxy(measureEditor, scoreLayoutDictionary, commandManager, notifyEntityChanged);
        }

        public static InstrumentMeasureEditorProxy ProxyEditor(this InstrumentMeasure measureEditor, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new InstrumentMeasureEditorProxy(measureEditor, scoreLayoutDictionary, commandManager, notifyEntityChanged);
        }





        public static MeasureBlockChainEditorProxy ProxyEditor(this MeasureBlockChain measureEditor, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new MeasureBlockChainEditorProxy(measureEditor, scoreLayoutDictionary, commandManager, notifyEntityChanged);
        }

        public static MeasureBlockEditorProxy ProxyEditor(this MeasureBlock chordGroup, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new MeasureBlockEditorProxy(chordGroup, scoreLayoutDictionary, commandManager, notifyEntityChanged);
        }

        public static ChordEditorProxy ProxyEditor(this Chord chordEditor, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new ChordEditorProxy(chordEditor, scoreLayoutDictionary, commandManager, notifyEntityChanged);
        }

        public static NoteEditorProxy ProxyEditor(this Note noteEditor, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new NoteEditorProxy(noteEditor, scoreLayoutDictionary, commandManager, notifyEntityChanged);
        }






        public static StaffSystemEditorProxy ProxyEditor(this StaffSystem staff, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new StaffSystemEditorProxy(staff, scoreLayoutDictionary, commandManager, notifyEntityChanged);
        }

        public static StaffGroupEditorProxy ProxyEditor(this StaffGroup staff, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new StaffGroupEditorProxy(staff, scoreLayoutDictionary, commandManager, notifyEntityChanged);
        }

        public static StaffEditorProxy ProxyEditor(this Staff staff, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new StaffEditorProxy(staff, scoreLayoutDictionary, commandManager, notifyEntityChanged);
        }
    }
}
