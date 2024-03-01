using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class ScoreMeasure : ScoreElement, IMementoElement<ScoreMeasureMemento>
    {
        private readonly ScoreDocumentCore score;

        public TimeSignature TimeSignature { get; }
        public KeySignature KeySignature { get; set; }


        public int IndexInScore =>
            score.contentTable.IndexOf(this);
        public bool IsLastInScore =>
            IndexInScore == score.NumberOfMeasures - 1;



        internal ScoreMeasure(ScoreDocumentCore score, TimeSignature timeSignature, KeySignature keySignature, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.score = score;

            TimeSignature = timeSignature;
            KeySignature = keySignature;
        }



        public void EditKeySignature(KeySignature keySignature)
        {
            if(this.KeySignature.Equals(keySignature)) return;

            this.KeySignature = keySignature;
        }
        public IEnumerable<InstrumentMeasure> EnumerateMeasuresCore()
        {
            var measures = score.contentTable.GetInstrumentMeasuresInScoreMeasure(IndexInScore);
            return measures;
        }
        public InstrumentMeasure GetMeasureCore(int ribbonIndex)
        {
            return score.contentTable.GetInstrumentMeasure(IndexInScore, ribbonIndex);
        }
        public bool TryReadPrevious([NotNullWhen(true)] out ScoreMeasure? previous)
        {
            previous = null;
            if (IndexInScore == 0)
            {
                return false;
            }

            try
            {
                previous = score.contentTable.ScoreMeasureAt(IndexInScore - 1);
            }
            catch
            {

            }

            return previous is not null;
        }
        public bool TryReadNext([NotNullWhen(true)] out ScoreMeasure? next)
        {
            next = null;
            if (IndexInScore + 1 >= score.NumberOfMeasures)
            {
                return false;
            }

            try
            {
                next = score.contentTable.ScoreMeasureAt(IndexInScore + 1);
            }
            catch { }

            return next is not null;
        }


        public ScoreMeasureMemento GetMemento()
        {
            return new ScoreMeasureMemento
            {
                Measures = EnumerateMeasuresCore().Select(e => e.GetMemento()).ToList(),
                TimeSignature = TimeSignature,
                Guid = Guid
            };
        }
        public void ApplyMemento(ScoreMeasureMemento memento)
        {
            foreach (var measureMemento in memento.Measures)
            {
                var measure = GetMeasureCore(measureMemento.RibbonIndex);
                measure.ApplyMemento(measureMemento);
            }
        }


        public override IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return EnumerateMeasuresCore();
        }
    }
}
