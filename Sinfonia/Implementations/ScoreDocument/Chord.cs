using Sinfonia.Implementations.ScoreDocument.Converters;
using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;
using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.Primitives.Extensions;

namespace Sinfonia.Implementations.ScoreDocument
{
    public sealed class Chord : ScoreElement, IPositionElement, IMementoElement<ChordModel>, IBeamEditor
    {
        private readonly List<Note> measureElements;
        private readonly MeasureBlock hostBlock;
        private readonly ScoreDocumentStyleTemplate documentStyleTemplate;
        private readonly IKeyGenerator<int> keyGenerator;
        private readonly Dictionary<PowerOfTwo, BeamType> beamTypes = [];

        public Dictionary<PowerOfTwo, BeamType> BeamTypes => beamTypes;
        public RythmicDuration RythmicDuration { get; }
        public ChordLayout Layout { get; }
        public SecondaryChordLayout SecondaryLayout { get; }

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



        public Chord(MeasureBlock hostBlock,
                     RythmicDuration displayDuration,
                     ScoreDocumentStyleTemplate documentStyleTemplate,
                     ChordLayout chordLayout,
                     SecondaryChordLayout secondaryChordLayout,
                     IKeyGenerator<int> keyGenerator,
                     Guid guid) : base(keyGenerator, guid)
        {
            this.hostBlock = hostBlock;
            this.keyGenerator = keyGenerator;
            this.documentStyleTemplate = documentStyleTemplate;

            measureElements = [];

            RythmicDuration = displayDuration;
            Layout = chordLayout;
            SecondaryLayout = secondaryChordLayout;
        }





        public void Clear()
        {
            measureElements.Clear();
            Layout.Restore();
            SecondaryLayout.Restore();
        }
        public void Add(params Pitch[] pitches)
        {
            foreach (var pitch in pitches)
            {
                if (measureElements.Any(e => e.Pitch == pitch))
                {
                    continue;
                }

                var noteLayout = new NoteLayout(documentStyleTemplate.NoteStyleTemplate, Grace);
                var secondaryNoteLayout = new SecondaryNoteLayout(Guid.NewGuid(), noteLayout);
                Note noteInMeasure = new(pitch, this, noteLayout, secondaryNoteLayout, keyGenerator, Guid.NewGuid());
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



        public ChordModel GetMemento()
        {
            return new ChordModel
            {
                Id = Guid,
                Notes = measureElements.Select(n => n.GetMemento()).ToList(),
                RythmicDuration = RythmicDuration.Convert(),
                Layout = SecondaryLayout.GetMemento(),
                XOffset = Layout._XOffset.Field,
                Position = Position.Convert()
            };
        }
        public void ApplyMemento(ChordModel memento)
        {
            Clear();
            
            Layout.ApplyMemento(memento);
            SecondaryLayout.ApplyMemento(memento.Layout);

            foreach (var noteMemento in memento.Notes)
            {
                var pitch = noteMemento.Pitch.Convert();
                var noteLayout = new NoteLayout(documentStyleTemplate.NoteStyleTemplate, Grace);
                var secondaryLayout = new SecondaryNoteLayout(noteMemento.Layout?.Id ?? Guid.NewGuid(), noteLayout);
                var noteInMeasure = new Note(pitch, this, noteLayout, secondaryLayout, keyGenerator, noteMemento.Id);
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
            return beamTypes.TryGetValue(i, out var value) ? value : null;
        }
        public IEnumerable<(BeamType beam, PowerOfTwo duration)> GetBeamTypes()
        {
            return beamTypes.Select(e => (e.Value, new PowerOfTwo(e.Key)));
        }
    }
}
