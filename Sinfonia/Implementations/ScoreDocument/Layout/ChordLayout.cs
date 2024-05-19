using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.Models.Base;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    public abstract class BaseChordLayout
    {
        public abstract ValueTemplateProperty<double> _XOffset { get; }


        public double XOffset
        {
            get => _XOffset.Value;
            set => _XOffset.Value = value;
        }

        public void Restore()
        {
            _XOffset.Reset();
        }

        public void ApplyMemento(ChordLayoutMembers memento)
        {
            Restore();
            _XOffset.Field = memento.XOffset;
        }
        public void ApplyMemento(ChordLayoutModel memento)
        {
            ApplyMemento((ChordLayoutMembers)memento);
        }
    }

    public class ChordLayout : BaseChordLayout
    {
        public override ValueTemplateProperty<double> _XOffset { get; }


        public ChordLayout()
        {
            _XOffset = new ValueTemplateProperty<double>(() => 0);
        }
    }

    public class SecondaryChordLayout : BaseChordLayout, IChordLayout, ILayout<ChordLayoutModel>
    {
        public Guid Id { get; }
        public override ValueTemplateProperty<double> _XOffset { get; }


        public SecondaryChordLayout(ChordLayout source, Guid id)
        {
            Id = id;

            _XOffset = new ValueTemplateProperty<double>(() => source.XOffset);
        }

        public ChordLayoutModel GetMemento()
        {
            return new ChordLayoutModel()
            {
                Id = Id,
                XOffset = _XOffset.Field
            };
        }
    }
}