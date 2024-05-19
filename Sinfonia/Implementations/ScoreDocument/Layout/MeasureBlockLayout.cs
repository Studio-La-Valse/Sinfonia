using StudioLaValse.ScoreDocument.Layout.Templates;
using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.Models.Base;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    public abstract class BaseMeasureBlockLayout
    {
        public abstract ValueTemplateProperty<double> _StemLength { get; }
        public abstract ValueTemplateProperty<double> _BeamAngle { get; }

        public double StemLength { get => _StemLength.Value; set => _StemLength.Value = value; }
        public double BeamAngle { get => _BeamAngle.Value; set => _BeamAngle.Value = value; }


        public void Restore()
        {
            _StemLength.Reset();
            _BeamAngle.Reset();
        }

        public void ApplyMemento(MeasureBlockLayoutMembers? memento)
        {
            Restore();
            if(memento is null)
            {
                return;
            }

            _StemLength.Field = memento.StemLength;
            _BeamAngle.Field = memento.BeamAngle;
        }
        public void ApplyMemento(MeasureBlockLayoutModel? memento)
        {
            ApplyMemento(memento as MeasureBlockLayoutMembers);
        }
    }

    public class MeasureBlockLayout : BaseMeasureBlockLayout
    {
        public override ValueTemplateProperty<double> _StemLength { get; }
        public override ValueTemplateProperty<double> _BeamAngle { get; }


        public MeasureBlockLayout(MeasureBlockStyleTemplate styleTemplate)
        {
            _StemLength = new ValueTemplateProperty<double>(() => styleTemplate.StemLength);
            _BeamAngle = new ValueTemplateProperty<double>(() => styleTemplate.BeamAngle);
        }
    }

    public class SecondaryMeasureBlockLayout : BaseMeasureBlockLayout, IMeasureBlockLayout, ILayout<MeasureBlockLayoutModel>
    {
        public Guid Id { get; }

        public override ValueTemplateProperty<double> _StemLength { get; }
        public override ValueTemplateProperty<double> _BeamAngle { get; }


        public SecondaryMeasureBlockLayout(Guid id, MeasureBlockLayout blockLayout)
        {
            Id = id;

            _StemLength = new ValueTemplateProperty<double>(() => blockLayout.StemLength);
            _BeamAngle = new ValueTemplateProperty<double>(() => blockLayout.BeamAngle);
        }

        public MeasureBlockLayoutModel GetMemento()
        {
            return new MeasureBlockLayoutModel()
            {
                Id = Id,
                StemLength = _StemLength.Field,
                BeamAngle = _BeamAngle.Field,
            };
        }
    }
}
