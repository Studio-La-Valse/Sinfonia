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

    public class ParentMementoCommand<TEntity, TParent, TMemento> : BaseCommand where TParent : IMementoElement<TMemento>
    {
        private readonly TParent parent;
        private readonly TEntity entity;
        private readonly Action<TEntity> action;
        private TMemento? memento = default;

        public ParentMementoCommand(TParent parent, TEntity entity, Action<TEntity> action)
        {
            this.parent = parent;
            this.entity = entity;
            this.action = action;
        }

        public override void Do()
        {
            memento = parent.GetMemento();
            action(entity);
        }

        public override void Undo()
        {
            if (memento is null)
            {
                throw new UnreachableException("Memento not recorded; Undo() method called before do() method.");
            }

            parent.ApplyMemento(memento);
        }
    }
}
