using Sinfonia.Extensions;
using Sinfonia.Implementations.ScoreDocument.Layout.Elements;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class MeasureBlockChain : ScoreElement, IMementoElement<RibbonMeasureVoiceMemento>
    {
        private readonly List<MeasureBlock> blocks;
        private readonly IKeyGenerator<int> keyGenerator;


        public InstrumentMeasure RibbonMeasure { get; }
        public int Voice { get; }
        public Guid Guid { get; }



        public MeasureBlockChain(InstrumentMeasure ribbonMeasure, int voice, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator)
        {
            this.keyGenerator = keyGenerator;

            Guid = guid;
            blocks = [];
            RibbonMeasure = ribbonMeasure;
            Voice = voice;
        }




        public MeasureBlock? BlockRight(MeasureBlock block)
        {
            var index = IndexOfOrThrow(block);
            return blocks.ElementAtOrDefault(index + 1);
        }
        public MeasureBlock? BlockLeft(MeasureBlock block)
        {
            var index = IndexOfOrThrow(block);
            return blocks.ElementAtOrDefault(index - 1);
        }


        public int IndexOfOrThrow(MeasureBlock block)
        {
            var index = blocks.IndexOf(block);
            if (index == -1)
            {
                throw new Exception("Measure block does not exist in this measure chain");
            }

            return index;
        }


        public void Divide(params int[] steps)
        {
            if (steps.Length == 0)
            {
                throw new InvalidOperationException("Please provide at least one value");
            }

            if (blocks.Any(b => b.Containers.Any()))
            {
                throw new InvalidOperationException("Cannot divide this measure block chain because it already contains blocks that have content.");
            }

            foreach (var step in steps)
            {
                if (step <= 0)
                {
                    throw new InvalidOperationException("Please only provide steps greater than 0.");
                }
            }

            var timeSignature = RibbonMeasure.TimeSignature;
            var multiplier = timeSignature.Numinator;
            for (int i = 0; i < steps.Length; i++)
            {
                steps[i] *= multiplier;
            }

            var sum = steps.Sum();
            var denomMultiplier = sum / multiplier;
            var stepDenom = denomMultiplier * timeSignature.Denominator;

            var stepsAsRythmicDurations = steps.Select(step =>
            {
                var gcd = step.GCD(stepDenom);
                step /= gcd;

                var denom = stepDenom / gcd;
                var fraction = new Fraction(step, denom);
                if (!RythmicDuration.TryConstruct(fraction, out var rythmicDuration))
                {
                    throw new InvalidOperationException("Not all of the specified steps can be resolved to valid rythmic durations.");
                }

                return rythmicDuration;
            });

            if (stepsAsRythmicDurations.Sum().Decimal != timeSignature.Decimal)
            {
                throw new InvalidOperationException("The specified set of steps does not resolve to the same duration as the timesignature of the measure.");
            }

            blocks.Clear();
            foreach (var rythmicDuration in stepsAsRythmicDurations)
            {
                Append(rythmicDuration, false);
            }
        }
        public void DivideEqual(int number)
        {
            var steps = Enumerable.Range(0, number).Select(i => 1).ToArray();
            Divide(steps);
        }
        public void Prepend(RythmicDuration duration, bool grace)
        {
            if (!grace)
            {
                var newLength = blocks.Select(e => e.RythmicDuration).Sum() + duration;
                if (newLength > RibbonMeasure.TimeSignature)
                {
                    throw new Exception("New measure block cannot fit in this measure.");
                }
            }

            var layout = new MeasureBlockLayout();
            var newBlock = new MeasureBlock(duration, this, grace, layout, keyGenerator, Guid.NewGuid());
            blocks.Insert(0, newBlock);
        }
        public void Append(RythmicDuration duration, bool grace)
        {
            if (!grace)
            {
                var newLength = blocks.Select(e => e.RythmicDuration).Sum() + duration;
                if (newLength > RibbonMeasure.TimeSignature)
                {
                    throw new Exception("New measure block cannot fit in this measure.");
                }
            }

            var layout = new MeasureBlockLayout();
            var newBlock = new MeasureBlock(duration, this, grace, layout, keyGenerator, Guid.NewGuid());
            blocks.Add(newBlock);
        }
        public void Insert(Position position, RythmicDuration duration, bool grace)
        {
            if (!grace)
            {
                var newLength = blocks.Select(e => e.RythmicDuration).Sum() + duration;
                if (newLength > RibbonMeasure.TimeSignature)
                {
                    throw new Exception("New measure block cannot fit in this measure.");
                }
            }

            for (int i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];
                if (block.Position == position)
                {
                    var layout = new MeasureBlockLayout();
                    var newBlock = new MeasureBlock(duration, this, grace, layout, keyGenerator, Guid.NewGuid());
                    blocks.Insert(i, newBlock);
                    return;
                }
            }

            throw new Exception($"No existing block found that starts at position {position}");
        }
        public void Clear()
        {
            blocks.Clear();
        }


        public IEnumerable<MeasureBlock> GetBlocksCore()
        {
            return blocks;
        }




        public RibbonMeasureVoiceMemento GetMemento()
        {
            return new RibbonMeasureVoiceMemento
            {
                Voice = Voice,
                MeasureBlocks = blocks.Select(b => b.GetMemento()).ToList()
            };
        }
        public void ApplyMemento(RibbonMeasureVoiceMemento memento)
        {
            Clear();
            foreach (var block in memento.MeasureBlocks)
            {
                var duration = block.Duration;

                if (!block.Grace)
                {
                    var newLength = blocks.Select(e => e.RythmicDuration).Sum() + duration;
                    if (newLength > RibbonMeasure.TimeSignature)
                    {
                        throw new Exception("New measure block cannot fit in this measure.");
                    }
                }

                var layout = new MeasureBlockLayout();
                var newBlock = new MeasureBlock(duration, this, block.Grace, layout, keyGenerator, block.Guid);
                blocks.Add(newBlock);

                newBlock.ApplyMemento(block);
            }
        }


        public override IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return blocks;
        }
    }
}
