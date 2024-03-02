namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal class MeasureBlockChainReaderProxy : IMeasureBlockChainReader
    {
        private readonly MeasureBlockChain source;


        public int Voice => source.Voice;
        public Guid Guid => source.Guid;
        public int Id => source.Id;



        public MeasureBlockChainReaderProxy(MeasureBlockChain source)
        {
            this.source = source;
        }






        public IEnumerable<IMeasureBlockReader> ReadBlocks()
        {
            return source.GetBlocksCore().Select(e => e.Proxy());
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadBlocks();
        }
    }
}
