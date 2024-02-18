namespace Sinfonia.ViewModels.Application.Document.Inspector
{
    public class NotePropertiesViewModel : ScoreElementPropertiesViewModel
    {
        private readonly IEnumerable<INoteEditor> notes;
        private readonly IScoreBuilder scoreBuilder;

        internal NotePropertiesViewModel(IEnumerable<INoteEditor> notes, IScoreBuilder scoreBuilder) : base(notes)
        {
            this.notes = notes;
            this.scoreBuilder = scoreBuilder;
        }

        public override void Rebuild()
        {
            Properties.Add(Create(notes, n => n.XOffset, (n, v) => n.XOffset = v, scoreBuilder, "X Offset"));
        }
    }
}
