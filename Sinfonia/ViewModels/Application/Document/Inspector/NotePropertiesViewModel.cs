
namespace Sinfonia.ViewModels.Application.Document.Inspector
{
    public class NotePropertiesViewModel : ScoreElementPropertiesViewModel<INoteReader, INoteEditor, NoteLayout>
    {
        internal NotePropertiesViewModel(IEnumerable<INoteReader> notes, IScoreBuilder scoreBuilder, IScoreDocumentLayout scoreLayoutDictionary) : base(scoreBuilder, scoreLayoutDictionary, notes)
        {
            Properties.Add(Create(l => l.StaffIndex, (l, v) => l.StaffIndex = v, "Staff Index"));
            Properties.Add(Create(l => l.XOffset, (l, v) => l.XOffset = v, "X Offset"));
            Properties.Add(Create(l => l.ForceAccidental, (l, v) => l.ForceAccidental = v, "Accidental"));
            Properties.Add(Create(l => l.Scale, (l, v) => l.Scale = v, "Scale"));
        }

        public override string Header => "Note Properties";

        public override NoteLayout GetLayout(IScoreDocumentLayout scoreLayoutProvider, INoteReader entity)
        {
            return scoreLayoutProvider.NoteLayout(entity);
        }
    }
}
