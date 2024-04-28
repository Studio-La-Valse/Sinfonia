using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;
using StudioLaValse.ScoreDocument.Primitives.Extensions;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal sealed class Chord : ScoreElement, IPositionElement, IMementoElement<ChordMemento>
    {
        private readonly List<Note> measureElements;
        private readonly MeasureBlock hostBlock;
        private readonly ScoreDocumentStyleTemplate documentStyleTemplate;
        private readonly IKeyGenerator<int> keyGenerator;
        private readonly Dictionary<int, BeamType> beamTypes = [];


        public RythmicDuration RythmicDuration { get; }
        public ChordLayout Layout { get; }


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
        public InstrumentMeasure HostMeasure =>
            hostBlock.RibbonMeasure;


        public Chord(MeasureBlock hostBlock, RythmicDuration displayDuration, ScoreDocumentStyleTemplate documentStyleTemplate, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.hostBlock = hostBlock;
            this.keyGenerator = keyGenerator;
            this.documentStyleTemplate = documentStyleTemplate;

            measureElements = [];

            RythmicDuration = displayDuration;
            Layout = new ChordLayout(documentStyleTemplate.ChordStyleTemplate);
        }





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

                Note noteInMeasure = new(pitch, this, documentStyleTemplate, keyGenerator, Guid.NewGuid());
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
                RythmicDuration = RythmicDuration,
                Guid = Guid
            };
        }
        public void ApplyMemento(ChordMemento memento)
        {
            Clear();
            foreach (var noteMemento in memento.Notes)
            {
                var pitch = noteMemento.Pitch;
                Note noteInMeasure = new(pitch, this, documentStyleTemplate, keyGenerator, noteMemento.Guid);
                measureElements.Add(noteInMeasure);
                noteInMeasure.ApplyMemento(noteMemento);
            }
        }




        public void ClearBeams()
        {
            beamTypes.Clear();
        }
        public void SetBeamType(PowerOfTwo flag, BeamType beamType)
        {
            beamTypes[flag.Value] = beamType;
        }
        public bool TryGetBeamType(PowerOfTwo i, out BeamType? beamType)
        {
            beamType = null;

            if (beamTypes.TryGetValue(i.Value, out var _beamType))
            {
                beamType = _beamType;
                return true;
            }

            return false;
        }
        public BeamType? GetBeamType(PowerOfTwo i)
        {
            return beamTypes.TryGetValue(i.Value, out var value) ? value : null;
        }
        public IEnumerable<(BeamType beam, PowerOfTwo duration)> GetBeamTypes()
        {
            return beamTypes.Select(e => (e.Value, new PowerOfTwo(e.Key)));
        }
    }
}
