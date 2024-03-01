using IScoreLayoutDictionary = StudioLaValse.ScoreDocument.Layout.IScoreLayoutDictionary;

namespace Sinfonia.ViewModels.Application.Document.Inspector
{
    public class NotePropertiesViewModel : ScoreElementPropertiesViewModel
    {
        private readonly IEnumerable<INoteReader> notes;
        private readonly IScoreBuilder scoreBuilder;
        private readonly IScoreLayoutDictionary scoreLayoutDictionary;

        internal NotePropertiesViewModel(IEnumerable<INoteReader> notes, IScoreBuilder scoreBuilder, IScoreLayoutDictionary scoreLayoutDictionary) : base(notes)
        {
            this.notes = notes;
            this.scoreBuilder = scoreBuilder;
            this.scoreLayoutDictionary = scoreLayoutDictionary;
        }

        public override void Rebuild()
        {
            Properties.Clear();
            Properties.Add(
                CreateLayoutEditor(
                    notes,
                    (d, e) => d.GetOrDefault(e),
                    (l) => l.XOffset, 
                    (d, e, l) => d.Apply(e, l),
                    (l, p) => l.XOffset = p,
                    scoreBuilder,
                    scoreLayoutDictionary,
                    "X Offset")); ;
        }
    }
}
