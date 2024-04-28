using Sinfonia.Implementations.ScoreDocument.Memento.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    public class ScoreDocumentLayout : IScoreDocumentLayout, ILayout<ScoreDocumentLayoutMemento>
    {
        private readonly ScoreDocumentStyleTemplate styleTemplate;
        private readonly Dictionary<Instrument, double> instrumentScales = [];
        private readonly ValueTemplateProperty<double> scale;
        private readonly ValueTemplateProperty<double> horizontalStaffLineThickness;
        private readonly ValueTemplateProperty<double> verticalStaffLineThickness;
        private readonly ValueTemplateProperty<double> stemLineThickness;
        private readonly ValueTemplateProperty<double> firstSystemIndent;
        private readonly ReferenceTemplateProperty<ColorARGB> pageColor;
        private readonly ReferenceTemplateProperty<ColorARGB> foregroundColor;

        public double Scale
        {
            get
            {
                return scale.Value;
            }
            set
            {
                scale.Value = value;
            }
        }
        public double HorizontalStaffLineThickness
        {
            get
            {
                return horizontalStaffLineThickness.Value;
            }
            set
            {
                horizontalStaffLineThickness.Value = value;
            }
        }
        public double VerticalStaffLineThickness
        {
            get
            {
                return verticalStaffLineThickness.Value;
            }
            set
            {
                verticalStaffLineThickness.Value = value;
            }
        }
        public double StemLineThickness
        {
            get
            {
                return stemLineThickness.Value;
            }
            set
            {
                stemLineThickness.Value = value;
            }
        }
        public double FirstSystemIndent
        {
            get
            {
                return firstSystemIndent.Value;
            }
            set
            {
                firstSystemIndent.Value = value;
            }
        }
        public ColorARGB PageColor
        {
            get
            {
                return pageColor.Value;
            }
            set
            {
                pageColor.Value = value;
            }
        }
        public ColorARGB ForegroundColor
        {
            get
            {
                return foregroundColor.Value;
            }
            set
            {
                foregroundColor.Value = value;
            }
        }




        public ScoreDocumentLayout(ScoreDocumentStyleTemplate styleTemplate)
        {
            this.styleTemplate = styleTemplate;
            scale = new ValueTemplateProperty<double>(() => styleTemplate.Scale);
            horizontalStaffLineThickness = new ValueTemplateProperty<double>(() => styleTemplate.HorizontalStaffLineThickness);
            verticalStaffLineThickness = new ValueTemplateProperty<double>(() => styleTemplate.VerticalStaffLineThickness);
            stemLineThickness = new ValueTemplateProperty<double>(() => styleTemplate.StemLineThickness);
            firstSystemIndent = new ValueTemplateProperty<double>(() => styleTemplate.FirstSystemIndent);
            pageColor = new ReferenceTemplateProperty<ColorARGB>(() => styleTemplate.PageColor);
            foregroundColor = new ReferenceTemplateProperty<ColorARGB>(() => styleTemplate.ForegroundColor);
        }


        public void SpecifyScale(Instrument instrument, double scale)
        {
            instrumentScales[instrument] = scale;
        }
        public double GetInstrumentScale(Instrument instrument)
        {
            if(styleTemplate.InstrumentScales.TryGetValue(instrument, out var scale))
            {
                return scale;
            }

            if(instrumentScales.TryGetValue(instrument, out var value))
            {
                return value;
            }

            return 1;
        }


        public void ApplyMemento(ScoreDocumentLayoutMemento memento)
        {
            scale.Field = memento.Scale;
            horizontalStaffLineThickness.Field = memento.HorizontalStaffLineThickness;
            verticalStaffLineThickness.Field = memento.HorizontalStaffLineThickness;
            stemLineThickness.Field = memento.StemLineThickness;
            firstSystemIndent.Field = memento.FirstSystemIndent;
            pageColor.Field = memento.PageColor;
            foregroundColor.Field = memento.ForegroundColor;

            foreach(var kv in memento.InstrumentScales)
            {
                instrumentScales[kv.Key] = kv.Value;
            }
        }

        public void Restore()
        {
            scale.Reset();
            horizontalStaffLineThickness.Reset();
            verticalStaffLineThickness.Reset();
            stemLineThickness.Reset();
            firstSystemIndent.Reset();
            pageColor.Reset();
            foregroundColor.Reset();
        }

        public ScoreDocumentLayoutMemento GetMemento()
        {
            var dict = new Dictionary<Instrument, double>();
            foreach(var kv in instrumentScales)
            {
                dict[kv.Key] = kv.Value;
            }

            return new ScoreDocumentLayoutMemento()
            {
                Scale = scale.Field,
                HorizontalStaffLineThickness = horizontalStaffLineThickness.Field,
                VerticalStaffLineThickness = verticalStaffLineThickness.Field,
                StemLineThickness = stemLineThickness.Field,
                FirstSystemIndent = firstSystemIndent.Field,
                PageColor = pageColor.Field,
                ForegroundColor = foregroundColor.Field,
                InstrumentScales = dict
            };
        }
    }
}