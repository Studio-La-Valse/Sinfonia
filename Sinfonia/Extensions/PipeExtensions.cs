using Sinfonia.Implementations.Pipes;

namespace Sinfonia.Extensions
{
    public static class PipeExtensions
    {
        public static IPipe UndoRedo(this IPipe nextPipe, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            return new UndoRedoPipe(nextPipe, commandManager, notifyEntityChanged);
        }
    }
}
