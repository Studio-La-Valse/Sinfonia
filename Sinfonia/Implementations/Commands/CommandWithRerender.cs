namespace Sinfonia.Implementations.Commands
{
    public class CommandWithRerender<TEntity> : BaseCommand
    {
        private readonly BaseCommand baseCommand;
        private readonly INotifyEntityChanged<TEntity> entityChanged;
        private readonly TEntity entity;

        public CommandWithRerender(BaseCommand baseCommand, INotifyEntityChanged<TEntity> entityChanged, TEntity entity)
        {
            this.baseCommand = baseCommand;
            this.entityChanged = entityChanged;
            this.entity = entity;
        }

        public override void Do()
        {
            baseCommand.Do();
            entityChanged.Invalidate(entity);
        }

        public override void Undo()
        {
            baseCommand.Undo();
            entityChanged.Invalidate(entity);
        }
    }
}
