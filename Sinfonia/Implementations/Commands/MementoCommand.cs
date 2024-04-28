using System.Diagnostics;

namespace Sinfonia.Implementations.Commands
{
    public class MementoCommand<TEntity, TMemento> : BaseCommand where TEntity : IMementoElement<TMemento>
    {
        private readonly TEntity entity;
        private readonly Action<TEntity> action;
        private TMemento? memento = default;

        public MementoCommand(TEntity entity, Action<TEntity> action)
        {
            this.entity = entity;
            this.action = action;
        }

        public override void Do()
        {
            memento = entity.GetMemento();
            action(entity);
        }

        public override void Undo()
        {
            if (memento is null)
            {
                throw new UnreachableException("Memento not recorded; Undo() method called before do() method.");
            }

            entity.ApplyMemento(memento);
        }
    }

    public class RestoreLayoutCommand<TLayout, TMemento> : BaseCommand where TLayout : ILayout<TMemento>
    {
        private readonly TLayout layout;
        private TMemento? memento = default;

        public RestoreLayoutCommand(TLayout layout)
        {
            this.layout = layout;
        }

        public override void Do()
        {
            memento = layout.GetMemento();
            layout.Restore();
        }

        public override void Undo()
        {
            if (memento is null)
            {
                throw new UnreachableException("Memento not recorded; Undo() method called before do() method.");
            }

            layout.ApplyMemento(memento);
        }
    }
    public class LayoutMementoCommand<TLayout, TMemento> : BaseCommand where TLayout : ILayout<TMemento>
    {
        private readonly TLayout layout;
        private readonly Action<TMemento> action;
        private TMemento? memento = default;

        public LayoutMementoCommand(TLayout layout, Action<TLayout> action)
        {
            this.layout = layout;
            this.action = action;
        }

        public override void Do()
        {
            memento = layout.GetMemento();
            action(layout);
        }

        public override void Undo()
        {
            if (memento is null)
            {
                throw new UnreachableException("Memento not recorded; Undo() method called before do() method.");
            }

            layout.ApplyMemento(memento);
        }
    }
}
