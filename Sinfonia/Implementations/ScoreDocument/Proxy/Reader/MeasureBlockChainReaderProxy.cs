namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class MeasureBlockChainReaderProxy : IMeasureBlockChainReader
    {
        private readonly MeasureBlockChain source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;


        public int Voice => source.Voice;
        public Guid Guid => source.Guid;

        public int Id => source.Id;



        public MeasureBlockChainReaderProxy(MeasureBlockChain source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
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
    }
}
