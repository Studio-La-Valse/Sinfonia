using Sinfonia.Implementations.ScoreDocument.Converters;
using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;
using StudioLaValse.ScoreDocument.Models;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class Note : ScoreElement, IMementoElement<NoteModel>
    {
        private readonly Chord container;


        public Pitch Pitch { get; set; }
        public NoteLayout Layout { get; }
        public SecondaryNoteLayout SecondaryLayout { get; }

        public InstrumentMeasure HostMeasure => 
            container.HostMeasure;
        public bool Grace =>
            container.Grace;
        public Position Position =>
            container.Position;
        public RythmicDuration RythmicDuration =>
            container.RythmicDuration;
        public Tuplet Tuplet =>
            container.Tuplet;


        internal Note(Pitch pitch,
                      Chord container,
                      NoteLayout layout,
                      SecondaryNoteLayout secondaryLayout,
                      IKeyGenerator<int> keyGenerator,
                      Guid guid) : base(keyGenerator, guid)
        {
            this.container = container;

            Pitch = pitch;
            Layout = layout;
            SecondaryLayout = secondaryLayout;
        }



        public NoteModel GetMemento()
        {
            return new NoteModel
            {
                Pitch = Pitch.Convert(),
                Id = Guid,
                Layout = SecondaryLayout.GetMemento(),
                ForceAccidental = Layout._ForceAccidental.Field.HasValue ? (int)Layout._ForceAccidental.Field : null,
                Scale = Layout._Scale.Field,
                StaffIndex = Layout._StaffIndex.Field,
                XOffset = Layout._XOffset.Field,
            };
        }
        public void ApplyMemento(NoteModel memento)
        {
            Layout.ApplyMemento(memento);
            SecondaryLayout.ApplyMemento(memento.Layout);

            Pitch = memento.Pitch.Convert();
        }
    }
}