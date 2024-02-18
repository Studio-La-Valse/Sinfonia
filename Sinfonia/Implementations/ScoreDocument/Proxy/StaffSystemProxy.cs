namespace Sinfonia.Implementations.ScoreDocument.Proxy
{
    internal class StaffSystemProxy : IStaffSystemEditor, IStaffSystemReader
    {
        private readonly StaffSystem source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;


        public int Id => source.Id;

        public double PaddingTop
        {
            get => source.PaddingTop;
            set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<StaffSystem, StaffSystemMemento>(source, s => s.PaddingTop = value);
                transaction.Enqueue(command);
            }
        }



        public StaffSystemProxy(StaffSystem source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }



        public IStaffGroupEditor EditStaffGroup(int indexInScore)
        {
            return source.GetStaffGroupCore(indexInScore).Proxy(commandManager, notifyEntityChanged);
        }

        public IEnumerable<IStaffGroupEditor> EditStaffGroups()
        {
            return source.EnumerateStaffGroupsCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return source.EnumerateChildren();
        }

        public IEnumerable<IScoreMeasure> EnumerateMeasures()
        {
            return source.ReadMeasures().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IStaffGroup> EnumerateStaffGroups()
        {
            return source.EnumerateStaffGroupsCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public bool Equals(IUniqueScoreElement? other)
        {
            return source.Equals(other);
        }

        public IEnumerable<IScoreMeasureReader> ReadMeasures()
        {
            return source.ReadMeasures().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public IStaffGroupReader ReadStaffGroup(int indexInScore)
        {
            return source.GetStaffGroupCore(indexInScore).Proxy(commandManager, notifyEntityChanged);
        }

        public IEnumerable<IStaffGroupReader> ReadStaffGroups()
        {
            return source.EnumerateStaffGroupsCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }
    }
}
