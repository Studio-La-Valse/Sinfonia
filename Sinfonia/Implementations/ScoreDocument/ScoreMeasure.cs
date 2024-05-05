using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class ScoreMeasure : ScoreElement, IMementoElement<ScoreMeasureMemento>
    {
        private readonly ScoreDocumentCore score;
        private readonly ScoreDocumentStyleTemplate styleTemplate;

        public TimeSignature TimeSignature { get; }
        public ScoreMeasureLayout Layout { get; }


        public int IndexInScore =>
            score.IndexOf(this);
        public bool IsLastInScore =>
            IndexInScore == score.NumberOfMeasures - 1;
        public ScoreDocumentCore ScoreDocumentCore => 
            score;

        internal ScoreMeasure(ScoreDocumentCore score, TimeSignature timeSignature, ScoreDocumentStyleTemplate styleTemplate, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.score = score;
            this.styleTemplate = styleTemplate;

            TimeSignature = timeSignature;
            Layout = new ScoreMeasureLayout(styleTemplate.ScoreMeasureStyleTemplate, this);
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
