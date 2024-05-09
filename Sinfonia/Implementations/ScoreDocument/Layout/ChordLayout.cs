using Sinfonia.Implementations.ScoreDocument.Memento.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    public class ChordLayout : IChordLayout, ILayout<ChordLayoutMemento>
    {
        public Guid Id { get; }
        public double XOffset { get; set; }


        public ChordLayout(Guid id)
        {
            Id = id;
            XOffset = 0;
        }



        public ChordLayoutMemento GetMemento()
        {
            return new ChordLayoutMemento()
            {
                Id = Id,
                XOffset = XOffset
            };
        }

        public void ApplyMemento(ChordLayoutMemento memento)
        {
            XOffset = memento.XOffset ?? 0;
        }

        public void Restore()
        {
            XOffset = 0;
        }
    }
}