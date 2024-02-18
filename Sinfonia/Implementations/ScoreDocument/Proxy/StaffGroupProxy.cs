namespace Sinfonia.Implementations.ScoreDocument.Proxy
{
    internal class StaffGroupProxy : IStaffGroupEditor, IStaffGroupReader
    {
        private readonly StaffGroup source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;



        public int Id => source.Id;

        public int IndexInScore => source.IndexInScore;

        public Instrument Instrument => source.Instrument;

        public double DistanceToNext
        {
            get => source.DistanceToNext;
            set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<StaffGroup, StaffGroupMemento>(source, s => s.DistanceToNext = value);
                transaction.Enqueue(command);
            }
        }
        public int NumberOfStaves
        {
            get => source.NumberOfStaves;
            set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<StaffGroup, StaffGroupMemento>(source, s => s.NumberOfStaves = value);
                transaction.Enqueue(command);
            }
        }
        public bool Collapsed
        {
            get => source.Collapsed;
            set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<StaffGroup, StaffGroupMemento>(source, s => s.Collapsed = value);
                transaction.Enqueue(command);
            }
        }





        public StaffGroupProxy(StaffGroup source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }


        public IStaffEditor EditStaff(int staffIndex)
        {
            return source.GetStaffCore(staffIndex).Proxy(commandManager, notifyEntityChanged);
        }

        public IEnumerable<IStaffEditor> EditStaves()
        {
            return source.EnumerateStavesCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return source.EnumerateChildren();
        }

        public IEnumerable<IInstrumentMeasure> EnumerateMeasures()
        {
            return source.ReadMeasures().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IStaff> EnumerateStaves()
        {
            return source.EnumerateStavesCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public bool Equals(IUniqueScoreElement? other)
        {
            return source.Equals(other);
        }

        public IInstrumentRibbonReader ReadContext()
        {
            return source.ReadContext().Proxy(commandManager, notifyEntityChanged);
        }

        public IEnumerable<IInstrumentMeasureReader> ReadMeasures()
        {
            return source.ReadMeasures().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IStaffReader> ReadStaves()
        {
            return source.EnumerateStavesCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }
    }
}
