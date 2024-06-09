using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Reader;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class MeasureBlockChainReaderProxy : IMeasureBlockChainReader
    {
        private readonly MeasureBlockChain source;


        public int Voice => source.Voice;

        public int Id => source.Id;

        public TimeSignature TimeSignature => source.TimeSignature;

        public MeasureBlockChainReaderProxy(MeasureBlockChain source)
        {
            this.source = source;
        }






        public IEnumerable<IMeasureBlockReader> ReadBlocks()
        {
            return source.GetBlocksCore().Select(e => e.ProxyReader());
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadBlocks();
        }

        public override string ToString()
        {
            return $"Measure Block Chain : [{Voice}]";
        }
    }
}
