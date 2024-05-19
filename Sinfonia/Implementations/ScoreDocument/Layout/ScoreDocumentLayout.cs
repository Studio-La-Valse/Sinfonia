using StudioLaValse.ScoreDocument.Layout.Templates;
using StudioLaValse.ScoreDocument.Models;
using ColorARGB = StudioLaValse.ScoreDocument.Layout.Templates.ColorARGB;
using Sinfonia.Implementations.ScoreDocument.Converters;
using StudioLaValse.ScoreDocument.Models.Base;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    public abstract class BaseScoreDocumentLayout
    {
        public abstract ValueTemplateProperty<double> _Scale { get; }
        public abstract ValueTemplateProperty<double> _HorizontalStaffLineThickness { get; }
        public abstract ValueTemplateProperty<double> _VerticalStaffLineThickness { get; }
        public abstract ValueTemplateProperty<double> _StemLineThickness { get; }
        public abstract ValueTemplateProperty<double> _FirstSystemIndent { get; }
        public abstract ValueTemplateProperty<ColorARGB> _PageColor { get; }
        public abstract ValueTemplateProperty<ColorARGB> _ForegroundColor { get; }
        public Dictionary<Guid, double> _InstrumentScales { get; } = [];
        public abstract Dictionary<Guid, double> _InstrumentScalesSource { get; }

        public double Scale
        {
            get
            {
                return _Scale.Value;
            }
            set
            {
                _Scale.Value = value;
            }
        }
        public double HorizontalStaffLineThickness
        {
            get
            {
                return _HorizontalStaffLineThickness.Value;
            }
            set
            {
                _HorizontalStaffLineThickness.Value = value;
            }
        }
        public double VerticalStaffLineThickness
        {
            get
            {
                return _VerticalStaffLineThickness.Value;
            }
            set
            {
                _VerticalStaffLineThickness.Value = value;
            }
        }
        public double StemLineThickness
        {
            get
            {
                return _StemLineThickness.Value;
            }
            set
            {
                _StemLineThickness.Value = value;
            }
        }
        public double FirstSystemIndent
        {
            get
            {
                return _FirstSystemIndent.Value;
            }
            set
            {
                _FirstSystemIndent.Value = value;
            }
        }
        public ColorARGB PageColor
        {
            get
            {
                return _PageColor.Value;
            }
            set
            {
                _PageColor.Value = value;
            }
        }
        public ColorARGB ForegroundColor
        {
            get
            {
                return _ForegroundColor.Value;
            }
            set
            {
                _ForegroundColor.Value = value;
            }
        }


        public void SpecifyScale(IInstrumentRibbon instrument, double scale)
        {
            _InstrumentScales[instrument.Guid] = scale;
        }
        public double GetInstrumentScale(IInstrumentRibbon instrument)
        {
            if (_InstrumentScalesSource.TryGetValue(instrument.Guid, out var scale))
            {
                return scale;
            }

            if (_InstrumentScales.TryGetValue(instrument.Guid, out var value))
            {
                return value;
            }

            return 1;
        }


        public void Restore()
        {
            _Scale.Reset();
            _HorizontalStaffLineThickness.Reset();
            _VerticalStaffLineThickness.Reset();
            _StemLineThickness.Reset();
            _FirstSystemIndent.Reset();
            _PageColor.Reset();
            _ForegroundColor.Reset();
            _InstrumentScales.Clear();
        }
        public void ApplyMemento(ScoreDocumentLayoutMembers? memento)
        {
            Restore();
            if(memento is null)
            {
                return;
            }

            _Scale.Field = memento.Scale;
            _HorizontalStaffLineThickness.Field = memento.HorizontalStaffLineThickness;
            _VerticalStaffLineThickness.Field = memento.HorizontalStaffLineThickness;
            _StemLineThickness.Field = memento.StemLineThickness;
            _FirstSystemIndent.Field = memento.FirstSystemIndent;
            _PageColor.Field = memento.PageColor?.Convert();
            _ForegroundColor.Field = memento.ForegroundColor?.Convert();
            memento.InstrumentScales?.Replace(_InstrumentScales);
        }
        public void ApplyMemento(ScoreDocumentLayoutModel? memento)
        {
            ApplyMemento(memento as ScoreDocumentLayoutMembers);
        }
    }

    public class PrimaryScoreDocumentLayout : BaseScoreDocumentLayout
    {
        public override ValueTemplateProperty<double> _Scale { get; }
        public override ValueTemplateProperty<double> _HorizontalStaffLineThickness { get; }
        public override ValueTemplateProperty<double> _VerticalStaffLineThickness { get; }
        public override ValueTemplateProperty<double> _StemLineThickness { get; }
        public override ValueTemplateProperty<double> _FirstSystemIndent { get; }
        public override ValueTemplateProperty<ColorARGB> _PageColor { get; }
        public override ValueTemplateProperty<ColorARGB> _ForegroundColor { get; }
        public override Dictionary<Guid, double> _InstrumentScalesSource { get; }

        public PrimaryScoreDocumentLayout(ScoreDocumentStyleTemplate styleTemplate)
        {
            _Scale = new ValueTemplateProperty<double>(() => styleTemplate.Scale);
            _HorizontalStaffLineThickness = new ValueTemplateProperty<double>(() => styleTemplate.HorizontalStaffLineThickness);
            _VerticalStaffLineThickness = new ValueTemplateProperty<double>(() => styleTemplate.VerticalStaffLineThickness);
            _StemLineThickness = new ValueTemplateProperty<double>(() => styleTemplate.StemLineThickness);
            _FirstSystemIndent = new ValueTemplateProperty<double>(() => styleTemplate.FirstSystemIndent);
            _ForegroundColor = new ValueTemplateProperty<ColorARGB>(() => styleTemplate.ForegroundColor);
            _PageColor = new ValueTemplateProperty<ColorARGB>(() => styleTemplate.PageColor);
            _InstrumentScalesSource = styleTemplate.InstrumentScales;
        }
    }

    public class SecondaryScoreDocumentLayout : BaseScoreDocumentLayout, IScoreDocumentLayout, ILayout<ScoreDocumentLayoutModel> 
    {
        public override ValueTemplateProperty<double> _Scale { get; }
        public override ValueTemplateProperty<double> _HorizontalStaffLineThickness { get; }
        public override ValueTemplateProperty<double> _VerticalStaffLineThickness { get; }
        public override ValueTemplateProperty<double> _StemLineThickness { get; }
        public override ValueTemplateProperty<double> _FirstSystemIndent { get; }
        public override ValueTemplateProperty<ColorARGB> _PageColor { get; }
        public override ValueTemplateProperty<ColorARGB> _ForegroundColor { get; }
        public override Dictionary<Guid, double> _InstrumentScalesSource { get; }
        public Guid Id { get; }


        public SecondaryScoreDocumentLayout(PrimaryScoreDocumentLayout styleTemplate, Guid id)
        {
            _Scale = new ValueTemplateProperty<double>(() => styleTemplate.Scale);
            _HorizontalStaffLineThickness = new ValueTemplateProperty<double>(() => styleTemplate.HorizontalStaffLineThickness);
            _VerticalStaffLineThickness = new ValueTemplateProperty<double>(() => styleTemplate.VerticalStaffLineThickness);
            _StemLineThickness = new ValueTemplateProperty<double>(() => styleTemplate.StemLineThickness);
            _FirstSystemIndent = new ValueTemplateProperty<double>(() => styleTemplate.FirstSystemIndent);
            _ForegroundColor = new ValueTemplateProperty<ColorARGB>(() => styleTemplate.ForegroundColor);
            _PageColor = new ValueTemplateProperty<ColorARGB>(() => styleTemplate.PageColor);
            _InstrumentScalesSource = styleTemplate._InstrumentScalesSource;

            Id = id;
        }



        public ScoreDocumentLayoutModel GetMemento()
        {
            return new ScoreDocumentLayoutModel()
            {
                Id = Id,
                Scale = _Scale.Field,
                HorizontalStaffLineThickness = _HorizontalStaffLineThickness.Field,
                VerticalStaffLineThickness = _VerticalStaffLineThickness.Field,
                StemLineThickness = _StemLineThickness.Field,
                FirstSystemIndent = _FirstSystemIndent.Field,
                PageColor = _PageColor.Field?.Convert(),
                ForegroundColor = _ForegroundColor.Field?.Convert(),
                InstrumentScales = _InstrumentScalesSource.DeepCopy()
            };
        }
    }
}