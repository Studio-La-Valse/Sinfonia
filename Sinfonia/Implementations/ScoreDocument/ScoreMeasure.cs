using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class ScoreMeasure : ScoreElement, IMementoElement<ScoreMeasureMemento>
    {
        private readonly ScoreDocumentCore score;

        public TimeSignature TimeSignature { get; }
        public KeySignature KeySignature { get; set; }
        public ScoreMeasureLayout Layout { get; }


        public int IndexInScore =>
            score.IndexOf(this);
        public bool IsLastInScore =>
            IndexInScore == score.NumberOfMeasures - 1;


        internal ScoreMeasure(ScoreDocumentCore score, TimeSignature timeSignature, ScoreMeasureStyleTemplate styleTemplate, KeySignature keySignature, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.score = score;

            TimeSignature = timeSignature;
            KeySignature = keySignature;

            Layout = new ScoreMeasureLayout(styleTemplate, this);
        }



        public void EditKeySignature(KeySignature keySignature)
        {
            if (KeySignature.Equals(keySignature))
            {
                return;
            }

            KeySignature = keySignature;
        }
        public IEnumerable<InstrumentMeasure> EnumerateMeasuresCore()
        {
            var measures = score.EnumerateScoreMeasuresCore(this);
            return measures;
        }
        public InstrumentMeasure GetMeasureCore(int ribbonIndex)
        {
            return score.GetMeasureCore(IndexInScore, ribbonIndex);
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
                previous = score.GetScoreMeasureCore(IndexInScore - 1);
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
                next = score.GetScoreMeasureCore(IndexInScore + 1);
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
                Guid = Guid,
                IndexInScore = IndexInScore,
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
    }
}
