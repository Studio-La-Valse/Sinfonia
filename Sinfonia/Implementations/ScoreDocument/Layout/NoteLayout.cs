using Sinfonia.Implementations.ScoreDocument.Memento.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    /// <summary>
    /// The layout of a note.
    /// </summary>
    public class NoteLayout : INoteLayout, ILayout<NoteLayoutMemento>
    {
        private readonly NoteStyleTemplate styleTemplate;
        private readonly ValueTemplateProperty<AccidentalDisplay> forceAccidental;
        private readonly ValueTemplateProperty<double> scale;

        public int StaffIndex { get; set; }
        public double XOffset { get; set; }

        public AccidentalDisplay ForceAccidental 
        { 
            get => forceAccidental.Value; 
            set => forceAccidental.Value = value; 
        }
        public double Scale 
        { 
            get => scale.Value; 
            set => scale.Value = value; 
        }


        public NoteLayout(NoteStyleTemplate styleTemplate, bool grace)
        {
            this.styleTemplate = styleTemplate;

            forceAccidental = new ValueTemplateProperty<AccidentalDisplay>(() => this.styleTemplate.AccidentalDisplay);
            scale = new ValueTemplateProperty<double>(() => this.styleTemplate.Scale * (grace ? 0.5 : 1));

            StaffIndex = 0;
            XOffset = 0;
        }

        public NoteLayoutMemento GetMemento()
        {
            return new NoteLayoutMemento()
            {
                AccidentalDisplay = forceAccidental.Field,
                Scale = scale.Field,
                StaffIndex = StaffIndex,
                XOffset = XOffset
            };
        }

        public void ApplyMemento(NoteLayoutMemento memento)
        {
            StaffIndex = memento.StaffIndex;
            XOffset = memento.XOffset;
            forceAccidental.Field = memento.AccidentalDisplay;
            scale.Field = memento.Scale;
        }

        public void Restore()
        {
            StaffIndex = 0;
            XOffset = 0;
            forceAccidental.Field = null;
            scale.Field = null;
        }
    }
}