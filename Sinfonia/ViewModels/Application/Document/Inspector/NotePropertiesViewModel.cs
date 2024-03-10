namespace Sinfonia.ViewModels.Application.Document.Inspector
{
    public class NotePropertiesViewModel : ScoreElementPropertiesViewModel
    {
        private readonly IEnumerable<INoteReader> notes;

        internal NotePropertiesViewModel(IEnumerable<INoteReader> notes, IScoreBuilder scoreBuilder, IScoreLayoutProvider scoreLayoutDictionary) : base(notes, scoreBuilder, scoreLayoutDictionary)
        {
            this.notes = notes;
        }

        public override string Header => "Note Properties";

        public override void Rebuild()
        {
            Properties.Clear();
            Properties.Add(Create<double, NoteLayout, INoteReader, INoteEditor>(notes, (e, l) => e.NoteLayout(l).XOffset, (l, v) => l.XOffset = v, "X Offset"));
        }
    }
}
