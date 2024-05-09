using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class Note : ScoreElement, IMementoElement<NoteMemento>
    {
        private readonly Chord container;


        public Pitch Pitch { get; set; }
        public NoteLayout Layout { get; }


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
                      IKeyGenerator<int> keyGenerator,
                      Guid guid) : base(keyGenerator, guid)
        {
            this.container = container;

            Pitch = pitch;
            Layout = layout;
        }





        public NoteMemento GetMemento()
        {
            return new NoteMemento
            {
                Pitch = Pitch,
                Id = Guid,
                Layout = Layout.GetMemento()
            };
        }
        public void ApplyMemento(NoteMemento memento)
        {
            Pitch = memento.Pitch;
            Layout.ApplyMemento(memento.Layout);
        }
    }
}