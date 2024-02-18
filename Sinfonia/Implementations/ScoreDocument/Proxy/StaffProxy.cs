namespace Sinfonia.Implementations.ScoreDocument.Proxy
{
    internal class StaffProxy : IStaffEditor, IStaffReader
    {
        private readonly Staff source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;

        public int IndexInStaffGroup => source.IndexInStaffGroup;

        public int Id => source.Id;

        public double LineSpacing
        {
            get => source.LineSpacing; set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<Staff, StaffMemento>(source, s => s.LineSpacing = value);
                transaction.Enqueue(command);
            }
        }
        public double DistanceToNext
        {
            get => source.DistanceToNext; set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<Staff, StaffMemento>(source, s => s.LineSpacing = value);
                transaction.Enqueue(command);
            }
        }


        public StaffProxy(Staff source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }



        public IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return source.EnumerateChildren();
        }

        public bool Equals(IUniqueScoreElement? other)
        {
            return source.Equals(other);
        }
    }
}
