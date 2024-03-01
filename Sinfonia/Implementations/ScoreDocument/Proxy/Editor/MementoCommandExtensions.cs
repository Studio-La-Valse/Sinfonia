namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    public static class MementoCommandExtensions
    {
        public static BaseCommand ThenRerender<TEntity>(this BaseCommand command, INotifyEntityChanged<TEntity> entityChanged, TEntity entity)
        {
            return new CommandWithRerender<TEntity>(command, entityChanged, entity);
        }
    }
}
