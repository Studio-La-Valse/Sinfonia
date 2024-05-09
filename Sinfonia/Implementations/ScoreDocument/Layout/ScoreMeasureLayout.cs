using Sinfonia.Implementations.ScoreDocument.Memento.Layout;
using StudioLaValse.ScoreDocument.Core;
using StudioLaValse.ScoreDocument.Layout.Templates;
using StudioLaValse.ScoreDocument.Primitives;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    public class ScoreMeasureLayout : IScoreMeasureLayout, ILayout<ScoreMeasureLayoutMemento>
    {
        private readonly Guid id;
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

        public Guid Id => this.id;

        internal ScoreMeasureLayout(Guid id, ScoreMeasureStyleTemplate scoreMeasureStyleTemplate)
        {
            this.id = id;

            keySignature = new ValueTemplateProperty<KeySignature>(() => new KeySignature(new Step(0, 0), MajorOrMinor.Major));
            width = new ValueTemplateProperty<double>(() => scoreMeasureStyleTemplate.Width);
            paddingLeft = new ValueTemplateProperty<double>(() => scoreMeasureStyleTemplate.PaddingLeft);
            paddingRight = new ValueTemplateProperty<double>(() => scoreMeasureStyleTemplate.PaddingRight);
            PaddingBottom = null;
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
                Id = Id,
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