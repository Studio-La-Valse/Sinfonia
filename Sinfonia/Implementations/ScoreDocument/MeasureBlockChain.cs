using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class MeasureBlockChain : ScoreElement, IMementoElement<RibbonMeasureVoiceMemento>
    {
        private readonly List<MeasureBlock> blocks;
        private readonly ScoreDocumentStyleTemplate scoreDocumentStyle;
        private readonly IKeyGenerator<int> keyGenerator;

        public InstrumentMeasure RibbonMeasure { get; }
        public int Voice { get; }
        public TimeSignature TimeSignature => RibbonMeasure.TimeSignature;
        


        public MeasureBlockChain(InstrumentMeasure ribbonMeasure, ScoreDocumentStyleTemplate scoreDocumentStyle, int voice, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.keyGenerator = keyGenerator;

            blocks = [];

            RibbonMeasure = ribbonMeasure;
            this.scoreDocumentStyle = scoreDocumentStyle;
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
            return index == -1 ? throw new Exception("Measure block does not exist in this measure chain") : index;
        }


        public void Divide(params int[] steps)
        {
            var stepsAsRythmicDurations = TimeSignature.Divide(steps);

            Clear();
            foreach (var rythmicDuration in stepsAsRythmicDurations)
            {
                Append(rythmicDuration, false);
            }
        }
        public void DivideEqual(int number)
        {
            var stepsAsRythmicDurations = TimeSignature.DivideEqual(number);

            Clear();
            foreach (var rythmicDuration in stepsAsRythmicDurations)
            {
                Append(rythmicDuration, false);
            }
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

            MeasureBlock newBlock = new(duration, this, scoreDocumentStyle, grace, keyGenerator, Guid.NewGuid());
            blocks.Insert(0, newBlock);
        }
        public void Append(RythmicDuration duration, bool grace)
        {
            Append(duration, grace, Guid.NewGuid());
        }
        public void Append(RythmicDuration duration, bool grace, Guid guid)
        {
            if (!grace)
            {
                var newLength = blocks.Select(e => e.RythmicDuration).Sum() + duration;
                if (newLength > RibbonMeasure.TimeSignature)
                {
                    throw new Exception("New measure block cannot fit in this measure.");
                }
            }

            MeasureBlock newBlock = new(duration, this, scoreDocumentStyle, grace, keyGenerator, guid);
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

            for (var i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];
                if (block.Position == position)
                {
                    MeasureBlock newBlock = new(duration, this, scoreDocumentStyle, grace, keyGenerator, Guid.NewGuid());
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
                Append(block.Duration, false, block.Id);
                var newBlock = blocks.Last();
                newBlock.ApplyMemento(block);
            }
        }
    }
}
