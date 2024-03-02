namespace Sinfonia.ViewModels.Application.Document.Inspector
{
    public class NotePropertiesViewModel : ScoreElementPropertiesViewModel
    {
        private readonly IEnumerable<INoteReader> notes;
        private readonly IScoreBuilder scoreBuilder;
        private readonly IScoreLayoutProvider scoreLayoutDictionary;

        internal NotePropertiesViewModel(IEnumerable<INoteReader> notes, IScoreBuilder scoreBuilder, IScoreLayoutProvider scoreLayoutDictionary) : base(notes)
        {
            this.notes = notes;
            this.scoreBuilder = scoreBuilder;
            this.scoreLayoutDictionary = scoreLayoutDictionary;
        }

        public override void Rebuild()
        {
            Properties.Clear();
            Properties.Add(
                Create<double, NoteLayout, INoteReader, INoteEditor>(
                    notes,
                    e => scoreLayoutDictionary.NoteLayout(e).XOffset,
                    (l, e) => l.NoteLayout(e),
                    (l, v) => l.XOffset = v,
                    (b, l, e) => b.Apply(e, l),
                    scoreBuilder,
                    "X Offset")); ;
        }
    }
}
