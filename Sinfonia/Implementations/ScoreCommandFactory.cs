using Sinfonia.Implementations.ScoreDocument.Proxy;

namespace Sinfonia.Implementations
{
    internal class ScoreCommandFactory : IScoreCommandFactory
    {
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;

        public ScoreCommandFactory(INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.notifyEntityChanged = notifyEntityChanged;
        }

        public BaseCommand SetNoteXOFfset(Note note, double xOffset, IUniqueScoreElement invalidate)
        {
            var command = new MementoCommand<Note, NoteMemento>(note, s => s.XOffset = xOffset).ThenRerender(notifyEntityChanged, invalidate);
            return command;
        }
    }
}
