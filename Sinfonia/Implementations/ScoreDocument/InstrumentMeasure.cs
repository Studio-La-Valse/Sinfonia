using Sinfonia.Implementations.ScoreDocument.Layout.Elements;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class InstrumentMeasure : ScoreElement, IMementoElement<InstrumentMeasureMemento>
    {
        private readonly Dictionary<int, MeasureBlockChain> blockChains;
        private readonly ScoreMeasure scoreMeasure;
        private readonly InstrumentRibbon hostRibbon;
        private readonly IKeyGenerator<int> keyGenerator;
        private IInstrumentMeasureLayout layout;



        public Guid Guid { get; }


        public int MeasureIndex =>
            scoreMeasure.IndexInScore;
        public int RibbonIndex =>
            hostRibbon.IndexInScore;
        public TimeSignature TimeSignature =>
            scoreMeasure.TimeSignature;
        public Instrument Instrument =>
            hostRibbon.Instrument;

        public IEnumerable<ClefChange> ClefChanges => ReadLayout().ClefChanges;


        internal InstrumentMeasure(ScoreMeasure scoreMeasure, InstrumentRibbon hostRibbon, IInstrumentMeasureLayout layout, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator)
        {
            this.scoreMeasure = scoreMeasure;
            this.hostRibbon = hostRibbon;
            this.keyGenerator = keyGenerator;
            this.layout = layout;

            blockChains = [];

            Guid = guid;
        }





        public ScoreMeasure ReadMeasureContext() =>
            scoreMeasure;


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


        public MeasureBlockChain GetBlockChainOrThrowCore(int voice)
        {
            if (blockChains.TryGetValue(voice, out var chain))
            {
                return chain;
            }
            throw new Exception($"No voice {voice} found.");
        }



        public IEnumerable<int> EnumerateVoices()
        {
            return blockChains.Select(c => c.Key);
        }




        public IInstrumentMeasureLayout ReadLayout()
        {
            return layout;
        }
        public void ApplyLayout(IInstrumentMeasureLayout layout)
        {
            this.layout = layout;
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
                Layout = ReadLayout().Copy(),
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
            ApplyLayout(memento.Layout);
            foreach (var voiceGroup in memento.VoiceGroups)
            {
                var blockChain = GetBlockChainOrThrowCore(voiceGroup.Voice);
                blockChain.ApplyMemento(voiceGroup);
            }
        }




        public void AddClefChange(ClefChange clefChange)
        {
            ReadLayout().AddClefChange(clefChange);
        }
        public void RemoveClefChange(ClefChange clefChange)
        {
            ReadLayout().RemoveClefChange(clefChange);
        }



        public override IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return blockChains.Values;
        }
    }
}
