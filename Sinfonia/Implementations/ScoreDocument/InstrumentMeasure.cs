using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class InstrumentMeasure : ScoreElement, IMementoElement<InstrumentMeasureMemento>
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
        public KeySignature KeySignature =>
            scoreMeasure.KeySignature;


        public InstrumentMeasureLayout Layout { get; }


        internal InstrumentMeasure(ScoreMeasure scoreMeasure, InstrumentRibbon hostRibbon, ScoreDocumentStyleTemplate documentStyleTemplate, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.scoreMeasure = scoreMeasure;
            this.hostRibbon = hostRibbon;
            this.documentStyleTemplate = documentStyleTemplate;
            this.keyGenerator = keyGenerator;

            blockChains = [];

            Layout = new InstrumentMeasureLayout(this);
        }





        public MeasureBlockChain GetBlockChainOrThrowCore(int voice)
        {
            return blockChains.TryGetValue(voice, out var chain) ? chain : throw new Exception($"No voice {voice} found.");
        }


        public void Clear()
        {
            blockChains.Clear();
        }
        public void RemoveVoice(int voice)
        {
            _ = blockChains.Remove(voice);
        }
        public void AddVoice(int voice)
        {
            var guid = Guid.NewGuid();
            _ = blockChains.TryAdd(voice, new MeasureBlockChain(this, voice, keyGenerator, guid));
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




        public InstrumentMeasureMemento GetMemento()
        {
            return new InstrumentMeasureMemento
            {
                Guid = Guid.NewGuid(),
                MeasureIndex = MeasureIndex,
                RibbonIndex = RibbonIndex,
                VoiceGroups = blockChains.Values.Select(v => v.GetMemento()).ToList(),
                Layout = Layout.GetMemento()
            };
        }
        public void ApplyMemento(InstrumentMeasureMemento memento)
        {
            Clear();
            foreach (var voiceGroup in memento.VoiceGroups)
            {
                AddVoice(voiceGroup.Voice);
                var blockChain = GetBlockChainOrThrowCore(voiceGroup.Voice);
                blockChain.ApplyMemento(voiceGroup);
            }
            Layout.ApplyMemento(memento.Layout);
        }
    }
}
