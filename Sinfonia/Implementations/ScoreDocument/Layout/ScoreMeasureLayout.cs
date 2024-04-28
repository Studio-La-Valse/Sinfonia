using StudioLaValse.ScoreDocument.Core;
using StudioLaValse.ScoreDocument.Primitives;
using StudioLaValse.ScoreDocument.Layout.Templates;
using Sinfonia.Implementations.ScoreDocument.Memento.Layout;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    public class ScoreMeasureLayout : IScoreMeasureLayout, ILayout<ScoreMeasureLayoutMemento>
    {
        private readonly ScoreMeasureStyleTemplate scoreMeasureStyleTemplate;
        private readonly ScoreMeasure scoreMeasure;
        private readonly ValueTemplateProperty<double> width;
        private readonly ValueTemplateProperty<KeySignature> keySignature;
        private readonly ValueTemplateProperty<double> paddingLeft;
        private readonly ValueTemplateProperty<double> paddingRight;
        
        public KeySignature KeySignature
        {
            get
            {
                return keySignature.Value;
            }
            set
            {
                keySignature.Value = value;
            }
        }
        public double PaddingLeft
        {
            get
            {
                return paddingLeft.Value;
            }
            set
            {
                paddingLeft.Value = value;
            }
        }
        public double PaddingRight
        {
            get
            {
                return paddingRight.Value;
            }
            set
            {
                paddingRight.Value = value;
            }
        }
        public double Width
        {
            get
            {
                return width.Value;
            }
            set
            {
                width.Value = value;
            }
        }
        public double? PaddingBottom { get; set; }

        internal ScoreMeasureLayout(ScoreMeasureStyleTemplate scoreMeasureStyleTemplate, ScoreMeasure scoreMeasure)
        {
            this.scoreMeasureStyleTemplate = scoreMeasureStyleTemplate;
            this.scoreMeasure = scoreMeasure;

            keySignature = new ValueTemplateProperty<KeySignature>(() => scoreMeasure.KeySignature);
            width = new ValueTemplateProperty<double>(() => scoreMeasureStyleTemplate.Width);
            paddingLeft = new ValueTemplateProperty<double>(() => scoreMeasureStyleTemplate.PaddingLeft);
            paddingRight = new ValueTemplateProperty<double>(() => scoreMeasureStyleTemplate.PaddingRight);
            PaddingBottom = null;
        }


        public ScoreMeasureLayout Copy()
        {
            var copy = new ScoreMeasureLayout(scoreMeasureStyleTemplate, scoreMeasure);
            copy.width.Field = width.Field;
            copy.paddingLeft.Field = paddingLeft.Field;
            copy.paddingRight.Field = paddingRight.Field;
            copy.keySignature.Field = keySignature.Field;
            copy.PaddingBottom = PaddingBottom;
            return copy;
        }

        public void Restore()
        {
            width.Reset();
            paddingLeft.Reset();
            paddingRight.Reset();
            keySignature.Reset();
            PaddingBottom = null;
        }

        public ScoreMeasureLayoutMemento GetMemento()
        {
            return new ScoreMeasureLayoutMemento()
            {
                KeySignature = keySignature.Field,
                PaddingBottom = PaddingBottom,
                PaddingLeft = paddingLeft.Field,
                PaddingRight = paddingRight.Field,
                Width = width.Field,
            };
        }

        public void ApplyMemento(ScoreMeasureLayoutMemento memento)
        {
            keySignature.Field = memento.KeySignature;
            PaddingBottom = memento.PaddingBottom;
            paddingLeft.Field = memento.PaddingLeft;
            paddingRight.Field = memento.PaddingRight;
            width.Field = memento.Width;
        }
    }
}