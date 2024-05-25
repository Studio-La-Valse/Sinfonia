using Sinfonia.Implementations.ScoreDocument.Converters;
using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Core;
using StudioLaValse.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;
using StudioLaValse.ScoreDocument.Models;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class MeasureBlock : ScoreElement, IPositionElement, IMementoElement<MeasureBlockModel>
    {
        private readonly List<Chord> chords;
        private readonly MeasureBlockChain host;
        private readonly ScoreDocumentStyleTemplate documentStyleTemplate;
        private readonly IKeyGenerator<int> keyGenerator;

        public Position Position
        {
            get
            {
                var index = host.IndexOfOrThrow(this);

                Position position = new(0, 4);

                foreach (var block in host.GetBlocksCore().Take(index))
                {
                    if (block.Grace)
                    {
                        continue;
                    }

                    position += block.RythmicDuration;
                }

                return position;
            }
        }
        public InstrumentMeasure RibbonMeasure =>
            host.RibbonMeasure;
        public IEnumerable<Chord> Containers =>
            chords;
        public Tuplet Tuplet
        {
            get
            {
                var groupLength = Containers.Select(e => e.RythmicDuration).ToArray();
                return new Tuplet(RythmicDuration, groupLength);
            }
        }


        public bool Grace { get; }
        public RythmicDuration RythmicDuration { get; }
        public MeasureBlockLayout Layout { get; }
        public SecondaryMeasureBlockLayout SecondaryLayout { get; }
        public InstrumentMeasure InstrumentMeasure =>
            host.RibbonMeasure;

        public MeasureBlock(RythmicDuration duration,
                            MeasureBlockChain host,
                            ScoreDocumentStyleTemplate documentStyleTemplate,
                            MeasureBlockLayout layout,
                            SecondaryMeasureBlockLayout secondaryLayout,
                            bool grace,
                            IKeyGenerator<int> keyGenerator,
                            Guid guid) : base(keyGenerator, guid)
        {
            this.host = host;
            this.documentStyleTemplate = documentStyleTemplate;
            this.keyGenerator = keyGenerator;

            chords = [];

            Grace = grace;
            RythmicDuration = duration;
            Layout = layout;
            SecondaryLayout = secondaryLayout;
        }



        public IEnumerable<Chord> GetChordsCore()
        {
            return chords;
        }


        public Chord? ContainerRight(Chord elementContainer)
        {
            var index = IndexOfOrThrow(elementContainer);
            return index - 1 < chords.Count ? chords[index + 1] : null;
        }
        public Chord? ContainerLeft(Chord elementContainer)
        {
            var index = IndexOfOrThrow(elementContainer);
            return index > 0 ? chords[index - 1] : null;
        }


        public int IndexOfOrThrow(Chord container)
        {
            var index = chords.IndexOf(container);

            return index == -1 ? throw new Exception("Measure element container does not exist in this measure block") : index;
        }

        public bool TryReadNext([NotNullWhen(true)] out MeasureBlock? right)
        {
            right = host.BlockRight(this);
            return right is not null;
        }
        public bool TryReadPrevious([NotNullWhen(true)] out MeasureBlock? previous)
        {
            previous = host.BlockLeft(this);
            return previous is not null;
        }





        public void Clear()
        {
            chords.Clear();
        }
        public void AppendChord(RythmicDuration rythmicDuration, bool rebeam=true)
        {
            var guid = Guid.NewGuid();
            var layoutGuid = Guid.NewGuid();
            var chordLayout = new ChordLayout();
            var secondaryChordLayout = new SecondaryChordLayout(chordLayout, layoutGuid);
            var chord = new Chord(this, rythmicDuration, documentStyleTemplate, chordLayout, secondaryChordLayout, keyGenerator, guid);
            chords.Add(chord);
            if (rebeam)
            {
                Rebeam();
            }
        }
        public void Splice(int index)
        {
            chords.RemoveAt(index);
            Rebeam();
        }


        public void Divide(params int[] steps)
        {
            var stepsAsRythmicDurations = RythmicDuration.Divide(steps);

            Clear();
            foreach (var rythmicDuration in stepsAsRythmicDurations)
            {
                AppendChord(rythmicDuration, rebeam: false);
            }
            Rebeam();
        }
        public void DivideEqual(int number)
        {
            var stepsAsRythmicDurations = RythmicDuration.DivideEqual(number);

            Clear();
            foreach (var rythmicDuration in stepsAsRythmicDurations)
            {
                AppendChord(rythmicDuration, rebeam: false);
            }
            Rebeam();
        }



        public void Rebeam()
        {
            new RebeamStrategy().Rebeam(chords);
        }




        public MeasureBlockModel GetMemento()
        {
            return new MeasureBlockModel()
            {
                Id = Guid,
                Chords = chords.Select(c => c.GetMemento()).ToList(),
                Duration = RythmicDuration.Convert(),
                Layout = SecondaryLayout.GetMemento(),
                Voice = host.Voice,
                BeamAngle = Layout._BeamAngle.Field,
                StemLength = Layout._StemLength.Field,
                Position = Position.Convert()
            };
        }

        public void ApplyMemento(MeasureBlockModel memento)
        {
            Clear();

            Layout.ApplyMemento(memento);
            SecondaryLayout.ApplyMemento(memento.Layout);

            foreach (var chordMemento in memento.Chords)
            {
                var chordLayout = new ChordLayout();
                var secondaryChordLayout = new SecondaryChordLayout(chordLayout, chordMemento.Layout?.Id ?? Guid.NewGuid());
                var chord = new Chord(this, chordMemento.RythmicDuration.Convert(), documentStyleTemplate, chordLayout, secondaryChordLayout, keyGenerator, chordMemento.Id);
                chords.Add(chord);
                chord.ApplyMemento(chordMemento);
            }

            Rebeam();
        }
    }
}


