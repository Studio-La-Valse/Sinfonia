
namespace Sinfonia.Implementations.ScoreDocument
{
    internal class Note : ScoreElement, IMementoElement<NoteMemento>
    {
        private readonly Chord container;


        public Pitch Pitch { get; set; }
        public bool Grace =>
            container.Grace;
        public Position Position =>
            container.Position;
        public RythmicDuration RythmicDuration =>
            container.RythmicDuration;
        public Tuplet Tuplet =>
            container.Tuplet;



        internal Note(Pitch pitch, Chord container, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.container = container;

            Pitch = pitch;
        }




        public Chord ReadContext()
        {
            return container;
        }

        public NoteMemento GetMemento()
        {
            return new NoteMemento
            {
                Pitch = Pitch,
                Guid = Guid
            };
        }
        public void ApplyMemento(NoteMemento memento)
        {
            Pitch = memento.Pitch;
        }
    }
}