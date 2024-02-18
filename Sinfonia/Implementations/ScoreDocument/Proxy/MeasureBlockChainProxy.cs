namespace Sinfonia.Implementations.ScoreDocument.Proxy
{
    internal class MeasureBlockChainProxy : IMeasureBlockChainEditor, IMeasureBlockChainReader
    {
        private readonly MeasureBlockChain source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;


        public int Voice => source.Voice;

        public int Id => source.Id;



        public MeasureBlockChainProxy(MeasureBlockChain source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }




        public void Append(RythmicDuration duration, bool grace)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<MeasureBlockChain, RibbonMeasureVoiceMemento>(source, s => s.Append(duration, grace));
            transaction.Enqueue(command);
        }

        public void Clear()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<MeasureBlockChain, RibbonMeasureVoiceMemento>(source, s => s.Clear());
            transaction.Enqueue(command);
        }

        public void Divide(params int[] steps)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<MeasureBlockChain, RibbonMeasureVoiceMemento>(source, s => s.Divide(steps));
            transaction.Enqueue(command);

        }

        public void DivideEqual(int number)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<MeasureBlockChain, RibbonMeasureVoiceMemento>(source, s => s.DivideEqual(number));
            transaction.Enqueue(command);
        }

        public IEnumerable<IMeasureBlockEditor> EditBlocks()
        {
            return source.GetBlocksCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IMeasureBlock> EnumerateBlocks()
        {
            return source.GetBlocksCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }


        public IEnumerable<IMeasureBlockReader> ReadBlocks()
        {
            return source.GetBlocksCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return source.EnumerateChildren();
        }

        public bool Equals(IUniqueScoreElement? other)
        {
            return source.Equals(other);
        }

        public void Insert(Position position, RythmicDuration duration, bool grace)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<MeasureBlockChain, RibbonMeasureVoiceMemento>(source, s => s.Insert(position, duration, grace));
            transaction.Enqueue(command);
        }

        public void Prepend(RythmicDuration duration, bool grace)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<MeasureBlockChain, RibbonMeasureVoiceMemento>(source, s => s.Prepend(duration, grace));
            transaction.Enqueue(command);
        }
    }
}
