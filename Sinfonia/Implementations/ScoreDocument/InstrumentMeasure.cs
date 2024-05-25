using Sinfonia.Implementations.ScoreDocument.Converters;
using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;
using StudioLaValse.ScoreDocument.Models;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class InstrumentMeasure : ScoreElement, IUniqueScoreElement, IMementoElement<InstrumentMeasureModel>
    {
        private readonly Dictionary<int, MeasureBlockChain> blockChains;
        private readonly ScoreMeasure scoreMeasure;
        private readonly InstrumentRibbon hostRibbon;
        private readonly ScoreDocumentStyleTemplate documentStyleTemplate;
        private readonly IKeyGenerator<int> keyGenerator;



        public int MeasureIndex =>
            scoreMeasure.IndexInScore;
        public int RibbonIndex =>
            hostRibbon.IndexInScore;
        public TimeSignature TimeSignature =>
            scoreMeasure.TimeSignature;
        public Instrument Instrument =>
            hostRibbon.Instrument;
        public ScoreMeasure ScoreMeasure => 
            scoreMeasure;


        public InstrumentMeasureLayout Layout { get; }
        public SecondaryInstrumentMeasureLayout SecondaryLayout { get; }

        internal InstrumentMeasure(
            ScoreMeasure scoreMeasure, 
            InstrumentRibbon hostRibbon, 
            ScoreDocumentStyleTemplate documentStyleTemplate, 
            InstrumentMeasureLayout layout, 
            SecondaryInstrumentMeasureLayout secondaryLayout,
            IKeyGenerator<int> keyGenerator, 
            Guid guid) : base(keyGenerator, guid)
        {
            this.scoreMeasure = scoreMeasure;
            this.hostRibbon = hostRibbon;
            this.documentStyleTemplate = documentStyleTemplate;
            this.keyGenerator = keyGenerator;

            blockChains = [];

            Layout = layout;
            SecondaryLayout = secondaryLayout;
        }





        public MeasureBlockChain GetBlockChainOrThrowCore(int voice)
        {
            return blockChains.TryGetValue(voice, out var chain) ? chain : throw new Exception($"No voice {voice} found.");
        }


        public void Clear()
        {
            blockChains.Clear();
            Layout.Restore();
            SecondaryLayout.Restore();
        }
        public void RemoveVoice(int voice)
        {
            _ = blockChains.Remove(voice);
        }
        public void AddVoice(int voice)
        {
            var guid = Guid.NewGuid();
            _ = blockChains.TryAdd(voice, new MeasureBlockChain(this, documentStyleTemplate, voice, keyGenerator, guid));
        }





        public IEnumerable<int> EnumerateVoices()
        {
            return blockChains.Select(c => c.Key);
        }






        public bool TryReadPrevious([NotNullWhen(true)] out InstrumentMeasure? previous)
        {
            previous = null;
            if (scoreMeasure.TryReadPrevious(out var previousScoreMeasure))
            {
                previous = previousScoreMeasure.GetMeasureCore(RibbonIndex);
                return true;
            }
            return false;
        }
        public bool TryReadNext([NotNullWhen(true)] out InstrumentMeasure? next)
        {
            next = null;
            if (scoreMeasure.TryReadNext(out var nextScoreMeasure))
            {
                next = nextScoreMeasure.GetMeasureCore(RibbonIndex);
                return true;
            }
            return false;
        }




        public InstrumentMeasureModel GetMemento()
        {
            return new InstrumentMeasureModel
            {
                Id = Guid,
                ScoreMeasureIndex = MeasureIndex,
                InstrumentRibbonIndex = RibbonIndex,
                MeasureBlocks = blockChains.Values.SelectMany(v => v.GetBlocksCore()).Select(b => b.GetMemento()).ToList(),
                Layout = SecondaryLayout.GetMemento(),
                ClefChanges = Layout._ClefChanges.Select(e => e.Convert()).ToList(),
                Collapsed = Layout._Collapsed.Field,
                NumberOfStaves = Layout._NumberOfStaves.Field,
                PaddingBottom = Layout._PaddingBottom.Field,
                StaffPaddingBottom = Layout._PaddingBottomForStaves.DeepCopy()
            };
        }
        public void ApplyMemento(InstrumentMeasureModel memento)
        {
            Clear();

            Layout.ApplyMemento(memento);
            SecondaryLayout.ApplyMemento(memento.Layout);

            foreach (var voiceGroup in memento.MeasureBlocks.GroupBy(e => e.Voice))
            {
                var voice = voiceGroup.Key;
                AddVoice(voice);
                var blockChain = GetBlockChainOrThrowCore(voice);

                blockChain.Clear();
                foreach (var block in voiceGroup)
                {
                    var newBlock = blockChain.AppendCore(block.Duration.Convert(), false, block.Id, block.Layout?.Id ?? Guid.NewGuid());
                    newBlock.ApplyMemento(block);
                }
            }
        }
    }
}
