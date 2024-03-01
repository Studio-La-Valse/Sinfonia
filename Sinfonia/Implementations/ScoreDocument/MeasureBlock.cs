using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class MeasureBlock : ScoreElement, IMementoElement<MeasureBlockMemento>, IPositionElement
    {
        private readonly List<Chord> chords;
        private readonly MeasureBlockChain host;
        private readonly bool grace;
        private readonly IKeyGenerator<int> keyGenerator;
        private readonly RythmicDuration duration;




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
            chords;
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


        public MeasureBlock(RythmicDuration duration, MeasureBlockChain host, bool grace, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.host = host;
            this.grace = grace;
            this.keyGenerator = keyGenerator;
            this.duration = duration;

            chords = [];
        }



        public Chord? ContainerRight(Chord elementContainer)
        {
            var index = IndexOfOrThrow(elementContainer);
            if (index - 1 < chords.Count)
            {
                return chords[index + 1];
            }

            return null;
        }
        public Chord? ContainerLeft(Chord elementContainer)
        {
            var index = IndexOfOrThrow(elementContainer);
            if (index > 0)
            {
                return chords[index - 1];
            }

            return null;
        }


        public int IndexOfOrThrow(Chord container)
        {
            var index = chords.IndexOf(container);

            if (index == -1)
            {
                throw new Exception("Measure element container does not exist in this measure block");
            }

            return index;
        }


        public IEnumerable<Chord> GetChordsCore()
        {
            return chords;
        }




        public void AppendChord(RythmicDuration rythmicDuration)
        {
            var chord = new Chord(this, rythmicDuration, keyGenerator, Guid.NewGuid());
            chords.Add(chord);
            Rebeam();
        }
        public void Splice(int index)
        {
            chords.RemoveAt(index);
            Rebeam();
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
            foreach (var chord in chords)
            {
                chord.ClearBeams();
            }

            for (int i = 8; i <= 64; i *= 2)
            {
                var duration = 1M / i;

                for (int j = 0; j < chords.Count; j++)
                {
                    var leftChord = j >= 1 ? chords[j - 1] : null;
                    var middleChord = chords[j];
                    var rightChord = j < chords.Count - 1 ? chords[j + 1] : null;
                    var middleChordBeams = middleChord.GetBeamTypes();

                    if (middleChord.RythmicDuration.PowerOfTwo < 1 / duration)
                    {
                        continue;
                    }

                    var receives = leftChord is not null && leftChord.RythmicDuration.PowerOfTwo >= 1 / duration;
                    var sends = rightChord is not null && rightChord.RythmicDuration.PowerOfTwo >= 1 / duration;

                    if (leftChord is null && rightChord is null)
                    {
                        middleChord.SetBeamType(i, BeamType.Flag);
                        continue;
                    }

                    if (receives && sends)
                    {
                        middleChord.SetBeamType(i, BeamType.Continue);
                        continue;
                    }
                    else if (sends)
                    {
                        middleChord.SetBeamType(i, BeamType.Start);
                        continue;
                    }
                    else if (receives)
                    {
                        middleChord.SetBeamType(i, BeamType.End);
                        continue;
                    }
                    else if (leftChord is null)
                    {
                        middleChord.SetBeamType(i, BeamType.HookStart);
                        continue;
                    }
                    else if (rightChord is null)
                    {
                        middleChord.SetBeamType(i, BeamType.HookEnd);
                        continue;
                    }
                    else
                    {
                        if (!middleChord.TryGetBeamType(i / 2, out var beamUp))
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

                        middleChord.SetBeamType(i, toAdd);
                        continue;
                    }

                    throw new UnreachableException("Incoherent beaming strategy");
                }
            }
        }







        public void Clear()
        {
            chords.Clear();
        }




        public MeasureBlockMemento GetMemento()
        {
            return new MeasureBlockMemento()
            {
                Chords = chords.Select(c => c.GetMemento()).ToList(),
                Duration = RythmicDuration,
                Grace = Grace,
                Guid = Guid
            };
        }
        public void ApplyMemento(MeasureBlockMemento memento)
        {
            Clear();
            foreach (var chordMemento in memento.Chords)
            {
                var chord = new Chord(this, chordMemento.RythmicDuration, keyGenerator, chordMemento.Guid);
                chords.Add(chord);

                chord.ApplyMemento(chordMemento);
            }
        }

        public override IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return chords;
        }
    }
}
