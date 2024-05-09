using Sinfonia.Implementations.ScoreDocument.Memento.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    public class MeasureBlockLayout : IMeasureBlockLayout, ILayout<MeasureBlockLayoutMemento>
    {
        private readonly ValueTemplateProperty<double> stemLength;
        private readonly ValueTemplateProperty<double> beamAngle;

        public double StemLength { get => stemLength.Value; set => stemLength.Value = value; }
        public double BeamAngle { get => beamAngle.Value; set => beamAngle.Value = value; }
        public Guid Id { get; }

        public MeasureBlockLayout(Guid id, MeasureBlockStyleTemplate styleTemplate)
        {
            stemLength = new ValueTemplateProperty<double>(() => styleTemplate.StemLength);
            beamAngle = new ValueTemplateProperty<double>(() => styleTemplate.BracketAngle);
            Id = id;
        }

        public MeasureBlockLayoutMemento GetMemento()
        {
            return new MeasureBlockLayoutMemento()
            {
                Id = Id,
                StemLength = stemLength.Field,
                BeamAngle = beamAngle.Field,
            };
        }

        public void ApplyMemento(MeasureBlockLayoutMemento memento)
        {
            Restore();
            stemLength.Field = memento.StemLength;
            beamAngle.Field = memento.BeamAngle;
        }

        public void Restore()
        {
            stemLength.Reset();
            beamAngle.Reset();
        }
    }
}
