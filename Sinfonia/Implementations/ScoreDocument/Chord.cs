using Sinfonia.Implementations.ScoreDocument.Layout;
using Sinfonia.Implementations.ScoreDocument.Layout.Elements;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal sealed class Chord : ScoreElement, IPositionElement, IMementoElement<ChordMemento>, ILayoutElement<IChordLayout>
    {
        private readonly List<Note> measureElements;
        private readonly MeasureBlock hostBlock;
        private readonly IKeyGenerator<int> keyGenerator;
        private IChordLayout layout;



        public RythmicDuration RythmicDuration { get; }
        public Guid Guid { get; }


        public Tuplet Tuplet =>
            hostBlock.Tuplet;
        public Position Position
        {
            get
            {
                if (hostBlock.Grace)
                {
                    return hostBlock.Position;
                }

                var index = hostBlock.IndexOfOrThrow(this);
                var position = hostBlock.Position;

                foreach (var container in hostBlock.Containers.Take(index))
                {
                    position += container.ActualDuration();
                }

                return position;
            }
        }
        public bool Grace =>
            hostBlock.Grace;
        public int IndexInBlock =>
            hostBlock.IndexOfOrThrow(this);
        public double XOffset
        {
            get
            {
                return layout.XOffset;
            }
            set
            {
                layout.XOffset = value;
            }
        }



        public Chord(MeasureBlock hostBlock, RythmicDuration displayDuration, ChordLayout layout, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator)
        {
            this.hostBlock = hostBlock;
            this.keyGenerator = keyGenerator;
            this.layout = layout;

            measureElements = [];
            RythmicDuration = displayDuration;
            Guid = guid;
        }


        public InstrumentMeasure ReadContext() =>
            hostBlock.RibbonMeasure;



        public void Clear()
        {
            measureElements.Clear();
        }
        public void Add(params Pitch[] pitches)
        {
            foreach (var pitch in pitches)
            {
                if (measureElements.Any(e => e.Pitch == pitch))
                {
                    continue;
                }

                var layout = new MeasureElementLayout();
                var noteInMeasure = new Note(pitch, this, layout, keyGenerator, Guid.NewGuid());
                measureElements.Add(noteInMeasure);
            }
        }
        public void Set(params Pitch[] pitches)
        {
            measureElements.Clear();

            Add(pitches);
        }



        public IEnumerable<Note> EnumerateNotesCore()
        {
            return measureElements;
        }



        public ChordMemento GetMemento()
        {
            return new ChordMemento
            {
                Notes = measureElements.Select(n => n.GetMemento()).ToList(),
                Layout = ReadLayout(),
                RythmicDuration = RythmicDuration,
                Guid = Guid
            };
        }
        public void ApplyMemento(ChordMemento memento)
        {
            Clear();
            ApplyLayout(memento.Layout);
            foreach (var noteMemento in memento.Notes)
            {
                var layout = noteMemento.Layout;
                var pitch = noteMemento.Pitch;
                var noteInMeasure = new Note(pitch, this, layout, keyGenerator, noteMemento.Guid);
                measureElements.Add(noteInMeasure);
            }
        }



        public IChordLayout ReadLayout()
        {
            return layout;
        }
        public void ApplyLayout(IChordLayout memento)
        {
            layout = memento;
        }



        public void SetBeamType(PowerOfTwo flag, BeamType beamType)
        {
            layout.Beams[flag.Value] = beamType;
        }
        public BeamType? GetBeamType(PowerOfTwo flag)
        {
            var value = layout.Beams.TryGetValue(flag, out var _value);
            if (value)
            {
                return _value;
            }
            return null;
        }
        public IEnumerable<(BeamType beam, PowerOfTwo duration)> GetBeamTypes()
        {
            return layout.Beams.Select(e => (e.Value, e.Key));
        }




        public override IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return measureElements;
        }
    }
}
