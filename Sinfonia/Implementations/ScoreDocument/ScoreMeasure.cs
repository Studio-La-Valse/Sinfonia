using Sinfonia.Implementations.ScoreDocument.Converters;
using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;
using StudioLaValse.ScoreDocument.Models;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class ScoreMeasure : ScoreElement, IMementoElement<ScoreMeasureModel>
    {
        private readonly ScoreDocumentCore score;


        public TimeSignature TimeSignature { get; }
        public PrimaryScoreMeasureLayout Layout { get; }
        public SecondaryScoreMeasureLayout SecondaryLayout { get; }


        public int IndexInScore =>
            score.IndexOf(this);
        public bool IsLastInScore =>
            IndexInScore == score.NumberOfMeasures - 1;
        public ScoreDocumentCore ScoreDocumentCore => 
            score;



        internal ScoreMeasure(ScoreDocumentCore score,
                              TimeSignature timeSignature,
                              ScoreDocumentStyleTemplate styleTemplate,
                              PrimaryScoreMeasureLayout layout,
                              SecondaryScoreMeasureLayout secondaryLayout,
                              IKeyGenerator<int> keyGenerator,
                              Guid guid) : base(keyGenerator, guid)
        {
            this.score = score;

            TimeSignature = timeSignature;
            Layout = layout;
            SecondaryLayout = secondaryLayout;
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


        public ScoreMeasureModel GetMemento()
        {
            return new ScoreMeasureModel
            {
                Id = Guid,
                Layout = SecondaryLayout.GetMemento(),
                InstrumentMeasures = EnumerateMeasuresCore().Select(e => e.GetMemento()).ToList(),
                TimeSignature = TimeSignature.Convert(),
                IndexInScore = IndexInScore,
                KeySignature = Layout._KeySignature.Field?.Convert(),
                PaddingBottom = Layout._PaddingBottom.Field,
                PaddingLeft = Layout._PaddingLeft.Field,
                PaddingRight = Layout._PaddingRight.Field,
                Width = Layout._Width.Field
            };
        }
        public void ApplyMemento(ScoreMeasureModel memento)
        {
            Layout.ApplyMemento(memento);
            SecondaryLayout.ApplyMemento(memento.Layout);

            foreach (var measureMemento in memento.InstrumentMeasures)
            {
                var measure = GetMeasureCore(measureMemento.InstrumentRibbonIndex);
                measure.ApplyMemento(measureMemento);
            }
        }
    }
}
