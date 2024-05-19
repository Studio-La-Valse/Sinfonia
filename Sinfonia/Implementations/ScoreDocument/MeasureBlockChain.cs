using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Core;
using StudioLaValse.ScoreDocument.Layout.Templates;
using System;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class MeasureBlockChain : ScoreElement
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

            var layout = new MeasureBlockLayout(scoreDocumentStyle.MeasureBlockStyleTemplate);
            var secondaryLayout = new SecondaryMeasureBlockLayout(Guid.NewGuid(), layout);
            var newBlock = new MeasureBlock(duration, this, scoreDocumentStyle, layout, secondaryLayout, grace, keyGenerator, Guid.NewGuid());
            blocks.Insert(0, newBlock);
        }
        public void Append(RythmicDuration duration, bool grace)
        {
            var blockId = Guid.NewGuid();   
            var layoutId = Guid.NewGuid();
            AppendCore(duration, grace, blockId, layoutId);
        }
        public MeasureBlock AppendCore(RythmicDuration rythmicDuration, bool grace, Guid blockGuid, Guid secondaryLayoutGuid)
        {
            if (!grace)
            {
                ThrowIfWillCauseOverflow(rythmicDuration);
            }

            var layout = new MeasureBlockLayout(scoreDocumentStyle.MeasureBlockStyleTemplate);
            var secondaryLayout = new SecondaryMeasureBlockLayout(secondaryLayoutGuid, layout);
            var newBlock = new MeasureBlock(rythmicDuration, this, scoreDocumentStyle, layout, secondaryLayout, grace, keyGenerator, blockGuid);
            blocks.Add(newBlock);
            return newBlock;
        }
        public void Insert(Position position, RythmicDuration duration, bool grace)
        {
            if (!grace)
            {
                ThrowIfWillCauseOverflow(duration);
            }

            for (var i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];
                if (block.Position == position)
                {
                    var layout = new MeasureBlockLayout(scoreDocumentStyle.MeasureBlockStyleTemplate);
                    var secondaryBlockLayout = new SecondaryMeasureBlockLayout(Guid.NewGuid(), layout);
                    var newBlock = new MeasureBlock(duration, this, scoreDocumentStyle, layout, secondaryBlockLayout, grace, keyGenerator, Guid.NewGuid());
                    blocks.Insert(i, newBlock);
                    return;
                }
            }

            throw new Exception($"No existing block found that starts at position {position}");
        }
        public void ThrowIfWillCauseOverflow(RythmicDuration rythmicDuration)
        {
            var newLength = blocks.Select(e => e.RythmicDuration).Sum() + rythmicDuration;
            if (newLength > RibbonMeasure.TimeSignature)
            {
                throw new Exception("New measure block cannot fit in this measure.");
            }
        }
        public void Clear()
        {
            blocks.Clear();
        }


        public IEnumerable<MeasureBlock> GetBlocksCore()
        {
            return blocks;
        }
    }
}
