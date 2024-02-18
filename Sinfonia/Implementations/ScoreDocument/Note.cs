using Sinfonia.Implementations.ScoreDocument.Layout;
using Sinfonia.Implementations.ScoreDocument.Layout.Elements;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class Note : ScoreElement, IMementoElement<NoteMemento>, ILayoutElement<INoteLayout>
    {
        private readonly Chord container;
        private INoteLayout layout;



        public Pitch Pitch { get; set; }
        public Guid Guid { get; }



        public bool Grace =>
            container.Grace;
        public Position Position =>
            container.Position;
        public RythmicDuration RythmicDuration =>
            container.RythmicDuration;
        public Tuplet Tuplet =>
            container.Tuplet;

        public AccidentalDisplay ForceAccidental { get => ReadLayout().ForceAccidental; set => ReadLayout().ForceAccidental = value; }
        public int StaffIndex { get => ReadLayout().StaffIndex; set => ReadLayout().StaffIndex = value; }
        public double XOffset { get => ReadLayout().XOffset; set => ReadLayout().XOffset = value; }



        internal Note(Pitch pitch, Chord container, INoteLayout measureElementLayout, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator)
        {
            this.container = container;
            layout = measureElementLayout;

            Pitch = pitch;
            Guid = guid;
        }




        public Chord ReadContext() =>
            container;



        public INoteLayout ReadLayout()
        {
            return layout;
        }
        public void ApplyLayout(INoteLayout layout)
        {
            this.layout = layout;
        }

        public NoteMemento GetMemento()
        {
            return new NoteMemento
            {
                Pitch = Pitch,
                Layout = ReadLayout(),
                Guid = Guid
            };
        }
        public void ApplyMemento(NoteMemento memento)
        {
            ApplyLayout(memento.Layout);
            Pitch = memento.Pitch;
        }


        public override IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            yield break;
        }
    }
}