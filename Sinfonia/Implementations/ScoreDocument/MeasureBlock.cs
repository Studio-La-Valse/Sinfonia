using Sinfonia.Implementations.ScoreDocument.Layout.Elements;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class MeasureBlock : ScoreElement, IMementoElement<MeasureBlockMemento>, IPositionElement
    {
        private readonly List<Chord> containers;
        private readonly MeasureBlockChain host;
        private readonly bool grace;
        private readonly IKeyGenerator<int> keyGenerator;
        private readonly RythmicDuration duration;
        private IMeasureBlockLayout layout;



        public Guid Guid { get; }


        public int Voice =>
            host.Voice;
        public Position Position
        {
            get
            {
                var index = host.IndexOfOrThrow(this);

                var position = new Position(0, 4);

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
            containers;
        public bool Grace =>
            grace;
        public RythmicDuration RythmicDuration =>
            duration;
        public Tuplet Tuplet
        {
            get
            {
                var groupLength = Containers.Select(e => e.RythmicDuration).ToArray();
                return new Tuplet(RythmicDuration, groupLength);
            }
        }

        public double StemLength { get => ReadLayout().StemLength; set => ReadLayout().StemLength = value; }
        public double BeamAngle { get => ReadLayout().BeamAngle; set => ReadLayout().BeamAngle = value; }


        public MeasureBlock(RythmicDuration duration, MeasureBlockChain host, bool grace, IMeasureBlockLayout layout, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator)
        {
            this.host = host;
            this.grace = grace;
            this.keyGenerator = keyGenerator;
            this.duration = duration;
            this.layout = layout;

            containers = [];

            Guid = guid;
        }



        public Chord? ContainerRight(Chord elementContainer)
        {
            var index = IndexOfOrThrow(elementContainer);
            if (index - 1 < containers.Count)
            {
                return containers[index + 1];
            }

            return null;
        }
        public Chord? ContainerLeft(Chord elementContainer)
        {
            var index = IndexOfOrThrow(elementContainer);
            if (index > 0)
            {
                return containers[index - 1];
            }

            return null;
        }


        public int IndexOfOrThrow(Chord container)
        {
            var index = containers.IndexOf(container);

            if (index == -1)
            {
                throw new Exception("Measure element container does not exist in this measure block");
            }

            return index;
        }


        public IEnumerable<Chord> GetChordsCore()
        {
            return containers;
        }




        public void AppendChord(RythmicDuration rythmicDuration)
        {
            var layout = new ChordLayout();
            containers.Add(new Chord(this, rythmicDuration, layout, keyGenerator, Guid.NewGuid()));
        }
        public void Splice(int index)
        {
            containers.RemoveAt(index);
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


        public void Rebeam()
        {
            foreach (var chord in containers)
            {
                chord.ReadLayout().Beams.Clear();
            }

            for (int i = 8; i <= 64; i *= 2)
            {
                var duration = 1M / i;

                for (int j = 0; j < containers.Count; j++)
                {
                    var leftChord = j >= 1 ? containers[j - 1] : null;
                    var middleChord = containers[j];
                    var rightChord = j < containers.Count - 1 ? containers[j + 1] : null;
                    var middleChordBeams = middleChord.ReadLayout().Beams;

                    if (middleChord.RythmicDuration.PowerOfTwo < 1 / duration)
                    {
                        continue;
                    }

                    var receives = leftChord is not null && leftChord.RythmicDuration.PowerOfTwo >= 1 / duration;
                    var sends = rightChord is not null && rightChord.RythmicDuration.PowerOfTwo >= 1 / duration;

                    if (leftChord is null && rightChord is null)
                    {
                        middleChordBeams.Add(i, BeamType.Flag);
                        continue;
                    }

                    if (receives && sends)
                    {
                        middleChordBeams.Add(i, BeamType.Continue);
                        continue;
                    }
                    else if (sends)
                    {
                        middleChordBeams.Add(i, BeamType.Start);
                        continue;
                    }
                    else if (receives)
                    {
                        middleChordBeams.Add(i, BeamType.End);
                        continue;
                    }
                    else if (leftChord is null)
                    {
                        middleChordBeams.Add(i, BeamType.HookStart);
                        continue;
                    }
                    else if (rightChord is null)
                    {
                        middleChordBeams.Add(i, BeamType.HookEnd);
                        continue;
                    }
                    else
                    {
                        if (!middleChordBeams.TryGetValue(i / 2, out var beamUp))
                        {
                            throw new UnreachableException("Incoherent beaming strategy");
                        }

                        var toAdd = beamUp switch
                        {
                            BeamType.Continue => BeamType.HookStart,
                            BeamType.End => BeamType.HookEnd,
                            BeamType.HookEnd => BeamType.HookEnd,
                            BeamType.HookStart => BeamType.HookStart,
                            BeamType.Start => BeamType.HookStart,
                            _ => throw new UnreachableException("Incoherent beaming strategy")
                        };

                        middleChordBeams.Add(i, toAdd);
                        continue;
                    }

                    throw new UnreachableException("Incoherent beaming strategy");
                }
            }
        }




        public IMeasureBlockLayout ReadLayout()
        {
            return layout;
        }
        public void ApplyLayout(IMeasureBlockLayout layout)
        {
            this.layout = layout;
        }




        public void Clear()
        {
            containers.Clear();
        }




        public MeasureBlockMemento GetMemento()
        {
            return new MeasureBlockMemento()
            {
                Chords = containers.Select(c => c.GetMemento()).ToList(),
                Layout = ReadLayout(),
                Duration = RythmicDuration,
                Grace = Grace,
                Guid = Guid
            };
        }
        public void ApplyMemento(MeasureBlockMemento memento)
        {
            Clear();
            ApplyLayout(memento.Layout);
            foreach (var chordMemento in memento.Chords)
            {
                var layout = new ChordLayout();
                var chord = new Chord(this, chordMemento.RythmicDuration, layout, keyGenerator, chordMemento.Guid);
                containers.Add(chord);

                chord.ApplyMemento(chordMemento);
            }
        }

        public override IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return containers;
        }
    }
}
