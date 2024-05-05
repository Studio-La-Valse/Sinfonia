using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class Note : ScoreElement, IMementoElement<NoteMemento>
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


        internal Note(Pitch pitch, Chord container, ScoreDocumentStyleTemplate documentStyleTemplate, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.container = container;

            Pitch = pitch;
            Layout = new(documentStyleTemplate.NoteStyleTemplate, Grace);
        }





        public NoteMemento GetMemento()
        {
            return new NoteMemento
            {
                Pitch = Pitch,
                Guid = Guid,
                Layout = Layout.GetMemento()
            };
        }
        public void ApplyMemento(NoteMemento memento)
        {
            Pitch = memento.Pitch;

            if(memento.Layout is not null)
            {
                Layout.ApplyMemento(memento.Layout);
            }
        }
    }
}