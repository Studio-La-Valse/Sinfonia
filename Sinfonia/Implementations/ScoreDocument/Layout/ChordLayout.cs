using Sinfonia.Implementations.ScoreDocument.Memento.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    public class ChordLayout : IChordLayout, ILayout<ChordLayoutMemento>
    {
        private readonly ChordStyleTemplate styleTemplate;


        public double XOffset { get; set; }


        public ChordLayout(ChordStyleTemplate styleTemplate)
        {
            this.styleTemplate = styleTemplate;

            XOffset = 0;
        }



        public ChordLayout Copy()
        {
            var copy = new ChordLayout(styleTemplate)
            {
                XOffset = XOffset
            };
            return copy;
        }

        public ChordLayoutMemento GetMemento()
        {
            return new ChordLayoutMemento()
            {
                XOffset = XOffset
            };
        }

        public void ApplyMemento(ChordLayoutMemento memento)
        {
            XOffset = memento.XOffset;
        }

        public void Restore()
        {
            XOffset = 0;
        }
    }
}