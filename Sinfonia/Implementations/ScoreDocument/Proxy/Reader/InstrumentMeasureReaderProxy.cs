using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal class InstrumentMeasureReaderProxy : IInstrumentMeasureReader
    {
        private readonly InstrumentMeasure source;



        public int MeasureIndex => source.MeasureIndex;

        public int RibbonIndex => source.RibbonIndex;

        public TimeSignature TimeSignature => source.TimeSignature;

        public Instrument Instrument => source.Instrument;

        public int Id => source.Id;

        public Guid Guid => source.Guid;

        public KeySignature KeySignature => source.KeySignature;




        public InstrumentMeasureReaderProxy(InstrumentMeasure source)
        {
            this.source = source;
        }






        public IMeasureBlockChainReader ReadBlockChainAt(int voice)
        {
            return source.GetBlockChainOrThrowCore(voice).Proxy();
        }

        public bool TryReadPrevious([NotNullWhen(true)] out IInstrumentMeasureReader? previous)
        {
            _ = source.TryReadPrevious(out InstrumentMeasure? _previous);
            previous = _previous?.Proxy();
            return previous != null;
        }

        public bool TryReadNext([NotNullWhen(true)] out IInstrumentMeasureReader? next)
        {
            _ = source.TryReadNext(out InstrumentMeasure? _next);
            next = _next?.Proxy();
            return next != null;
        }

        public IEnumerable<int> ReadVoices()
        {
            return source.EnumerateVoices();
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadVoices().Select(ReadBlockChainAt);
        }
    }
}
