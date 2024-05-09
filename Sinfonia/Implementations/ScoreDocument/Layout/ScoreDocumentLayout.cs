using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sinfonia.Implementations.ScoreDocument.Memento.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;
using ColorARGB = StudioLaValse.ScoreDocument.Layout.Templates.ColorARGB;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    public class ScoreDocumentLayout : IScoreDocumentLayout, ILayout<ScoreDocumentLayoutMemento>
    {
        private readonly Guid id; 
        private readonly ScoreDocumentStyleTemplate styleTemplate;
        private readonly Dictionary<Guid, double> instrumentScales = [];
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

        public Guid Id => this.id;

        public ScoreDocumentLayout(Guid id, ScoreDocumentStyleTemplate styleTemplate)
        {
            this.id = id;
            this.styleTemplate = styleTemplate;

            scale = new ValueTemplateProperty<double>(() => styleTemplate.Scale);
            horizontalStaffLineThickness = new ValueTemplateProperty<double>(() => styleTemplate.HorizontalStaffLineThickness);
            verticalStaffLineThickness = new ValueTemplateProperty<double>(() => styleTemplate.VerticalStaffLineThickness);
            stemLineThickness = new ValueTemplateProperty<double>(() => styleTemplate.StemLineThickness);
            firstSystemIndent = new ValueTemplateProperty<double>(() => styleTemplate.FirstSystemIndent);
            pageColor = new ReferenceTemplateProperty<ColorARGB>(() => styleTemplate.PageColor);
            foregroundColor = new ReferenceTemplateProperty<ColorARGB>(() => styleTemplate.ForegroundColor);
        }


        public void SpecifyScale(IInstrumentRibbon instrument, double scale)
        {
            instrumentScales[instrument.Guid] = scale;
        }
        public double GetInstrumentScale(IInstrumentRibbon instrument)
        {
            if(styleTemplate.InstrumentScales.TryGetValue(instrument.Guid, out var scale))
            {
                return scale;
            }

            if(instrumentScales.TryGetValue(instrument.Guid, out var value))
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
            memento.InstrumentScales.Replace(instrumentScales);
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
            instrumentScales.Clear();
        }

        public ScoreDocumentLayoutMemento GetMemento()
        {
            return new ScoreDocumentLayoutMemento()
            {
                Id = Id,
                Scale = scale.Field,
                HorizontalStaffLineThickness = horizontalStaffLineThickness.Field,
                VerticalStaffLineThickness = verticalStaffLineThickness.Field,
                StemLineThickness = stemLineThickness.Field,
                FirstSystemIndent = firstSystemIndent.Field,
                PageColor = pageColor.Field,
                ForegroundColor = foregroundColor.Field,
                InstrumentScales = instrumentScales.DeepCopy()
            };
        }
    }

    public static class DictionaryExtensions
    {
        public static Dictionary<TKey, TValue> DeepCopy<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) where TKey : IEquatable<TKey> where TValue : struct
        {
            var dict = new Dictionary<TKey, TValue>();
            foreach (var kv in dictionary)
            {
                dict[kv.Key] = kv.Value;
            }
            return dict;
        }

        public static void Replace<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> target) where TKey : IEquatable<TKey> where TValue : struct
        {
            target.Clear();
            foreach (var kv in dictionary)
            {
                target[kv.Key] = kv.Value;
            }
        }
    }
}