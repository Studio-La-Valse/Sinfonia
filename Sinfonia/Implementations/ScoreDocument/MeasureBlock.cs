using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class MeasureBlock : ScoreElement, IMementoElement<MeasureBlockMemento>, IPositionElement
    {
        private readonly List<Chord> chords;
        private readonly MeasureBlockChain host;
        private readonly IKeyGenerator<int> keyGenerator;

        public Position Position
        {
            get
            {
                int index = host.IndexOfOrThrow(this);

                Position position = new(0, 4);

                foreach (MeasureBlock? block in host.GetBlocksCore().Take(index))
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
                RythmicDuration[] groupLength = Containers.Select(e => e.RythmicDuration).ToArray();
                return new Tuplet(RythmicDuration, groupLength);
            }
        }


        public bool Grace { get; }
        public RythmicDuration RythmicDuration { get; }


        public MeasureBlock(RythmicDuration duration, MeasureBlockChain host, bool grace, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.host = host;
            Grace = grace;
            this.keyGenerator = keyGenerator;
            RythmicDuration = duration;

            chords = [];
        }



        public IEnumerable<Chord> GetChordsCore()
        {
            return chords;
        }


        public Chord? ContainerRight(Chord elementContainer)
        {
            int index = IndexOfOrThrow(elementContainer);
            return index - 1 < chords.Count ? chords[index + 1] : null;
        }
        public Chord? ContainerLeft(Chord elementContainer)
        {
            int index = IndexOfOrThrow(elementContainer);
            return index > 0 ? chords[index - 1] : null;
        }


        public int IndexOfOrThrow(Chord container)
        {
            int index = chords.IndexOf(container);

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
        public void AppendChord(RythmicDuration rythmicDuration)
        {
            Chord chord = new(this, rythmicDuration, keyGenerator, Guid.NewGuid());
            chords.Add(chord);
            Rebeam();
        }
        public void Splice(int index)
        {
            chords.RemoveAt(index);
            Rebeam();
        }


        public void Divide(params int[] steps)
        {
            IEnumerable<RythmicDuration> stepsAsRythmicDurations = RythmicDuration.Divide(steps);

            Clear();
            foreach (RythmicDuration rythmicDuration in stepsAsRythmicDurations)
            {
                AppendChord(rythmicDuration);
            }
        }
        public void DivideEqual(int number)
        {
            IEnumerable<RythmicDuration> stepsAsRythmicDurations = RythmicDuration.DivideEqual(number);

            Clear();
            foreach (RythmicDuration rythmicDuration in stepsAsRythmicDurations)
            {
                AppendChord(rythmicDuration);
            }
        }



        public void Rebeam()
        {
            foreach (Chord chord in chords)
            {
                chord.ClearBeams();
            }

            for (int i = 8; i <= 64; i *= 2)
            {
                decimal duration = 1M / i;

                for (int j = 0; j < chords.Count; j++)
                {
                    Chord? leftChord = j >= 1 ? chords[j - 1] : null;
                    Chord middleChord = chords[j];
                    Chord? rightChord = j < chords.Count - 1 ? chords[j + 1] : null;
                    IEnumerable<(BeamType beam, PowerOfTwo duration)> middleChordBeams = middleChord.GetBeamTypes();

                    if (middleChord.RythmicDuration.PowerOfTwo < 1 / duration)
                    {
                        continue;
                    }

                    bool receives = leftChord is not null && leftChord.RythmicDuration.PowerOfTwo >= 1 / duration;
                    bool sends = rightChord is not null && rightChord.RythmicDuration.PowerOfTwo >= 1 / duration;

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
                        if (!middleChord.TryGetBeamType(i / 2, out BeamType? beamUp))
                        {
                            throw new UnreachableException("Incoherent beaming strategy");
                        }

                        BeamType toAdd = beamUp switch
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
            foreach (ChordMemento chordMemento in memento.Chords)
            {
                Chord chord = new(this, chordMemento.RythmicDuration, keyGenerator, chordMemento.Guid);
                chords.Add(chord);

                chord.ApplyMemento(chordMemento);
            }
        }
    }
}
