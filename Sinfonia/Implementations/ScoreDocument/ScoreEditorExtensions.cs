using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Implementation;
using Chord = StudioLaValse.ScoreDocument.Implementation.Chord;

namespace Sinfonia.Implementations.ScoreDocument
{
    public static class ScoreEditorExtensions
    {
        public static ScoreDocumentEditorProxy ProxyEditor(this ScoreDocumentCore score, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new ScoreDocumentEditorProxy(score, commandManager, notifyEntityChanged);
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

        public static GraceGroupEditorProxy ProxyEditor(this GraceGroup noteEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new GraceGroupEditorProxy(noteEditor, commandManager, notifyEntityChanged);
        }

        public static ChordEditorProxy ProxyEditor(this Chord chordEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new ChordEditorProxy(chordEditor, commandManager, notifyEntityChanged);
        }

        public static GraceChordEditorProxy ProxyEditor(this GraceChord noteEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new GraceChordEditorProxy(noteEditor, commandManager, notifyEntityChanged);
        }

        public static NoteEditorProxy ProxyEditor(this Note noteEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new NoteEditorProxy(noteEditor, commandManager, notifyEntityChanged);
        }

        public static GraceNoteEditorProxy ProxyEditor(this GraceNote noteEditor, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new GraceNoteEditorProxy(noteEditor, commandManager, notifyEntityChanged);
        }
    }
}
