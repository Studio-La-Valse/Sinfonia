using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class InstrumentMeasure : ScoreElement, IMementoElement<InstrumentMeasureMemento>
    {
        private readonly Dictionary<int, MeasureBlockChain> blockChains;
        private readonly ScoreMeasure scoreMeasure;
        private readonly InstrumentRibbon hostRibbon;
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



        internal InstrumentMeasure(ScoreMeasure scoreMeasure, InstrumentRibbon hostRibbon, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.scoreMeasure = scoreMeasure;
            this.hostRibbon = hostRibbon;
            this.keyGenerator = keyGenerator;

            blockChains = [];
        }





        public MeasureBlockChain GetBlockChainOrThrowCore(int voice)
        {
            return blockChains.TryGetValue(voice, out MeasureBlockChain? chain) ? chain : throw new Exception($"No voice {voice} found.");
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
            Guid guid = Guid.NewGuid();
            _ = blockChains.TryAdd(voice, new MeasureBlockChain(this, voice, keyGenerator, guid));
        }





        public IEnumerable<int> EnumerateVoices()
        {
            return blockChains.Select(c => c.Key);
        }






        public bool TryReadPrevious([NotNullWhen(true)] out InstrumentMeasure? previous)
        {
            previous = null;
            if (scoreMeasure.TryReadPrevious(out ScoreMeasure? previousScoreMeasure))
            {
                previous = previousScoreMeasure.GetMeasureCore(RibbonIndex);
                return true;
            }
            return false;
        }
        public bool TryReadNext([NotNullWhen(true)] out InstrumentMeasure? next)
        {
            next = null;
            if (scoreMeasure.TryReadNext(out ScoreMeasure? nextScoreMeasure))
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
                VoiceGroups = blockChains.Values
                    .Select(v => v.GetMemento())
                    .ToList()
            };
        }
        public void ApplyMemento(InstrumentMeasureMemento memento)
        {
            Clear();
            foreach (RibbonMeasureVoiceMemento voiceGroup in memento.VoiceGroups)
            {
                MeasureBlockChain blockChain = GetBlockChainOrThrowCore(voiceGroup.Voice);
                blockChain.ApplyMemento(voiceGroup);
            }
        }
    }
}
