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
            if (blockChains.TryGetValue(voice, out var chain))
            {
                return chain;
            }
            throw new Exception($"No voice {voice} found.");
        }


        public void Clear()
        {
            blockChains.Clear();
        }
        public void RemoveVoice(int voice)
        {
            blockChains.Remove(voice);
        }
        public void AddVoice(int voice)
        {
            var guid = Guid.NewGuid();
            blockChains.TryAdd(voice, new MeasureBlockChain(this, voice, keyGenerator, guid));
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
                VoiceGroups = blockChains.Values
                    .Select(v => v.GetMemento())
                    .ToList()
            };
        }
        public void ApplyMemento(InstrumentMeasureMemento memento)
        {
            Clear();
            foreach (var voiceGroup in memento.VoiceGroups)
            {
                var blockChain = GetBlockChainOrThrowCore(voiceGroup.Voice);
                blockChain.ApplyMemento(voiceGroup);
            }
        }
    }
}
