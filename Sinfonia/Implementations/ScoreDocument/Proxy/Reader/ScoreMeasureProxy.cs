using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal class ScoreMeasureReaderProxy : IScoreMeasureReader
    {
        private readonly ScoreMeasure source;



        public int IndexInScore => source.IndexInScore;

        public Guid Guid => source.Guid;

        public TimeSignature TimeSignature => source.TimeSignature;

        public bool IsLastInScore => source.IsLastInScore;

        public int Id => source.Id;

        public KeySignature KeySignature => source.KeySignature;



        public ScoreMeasureReaderProxy(ScoreMeasure source)
        {
            this.source = source;
        }



        public bool TryReadNext([NotNullWhen(true)] out IScoreMeasureReader? next)
        {
            source.TryReadNext(out var _next);
            next = _next?.Proxy();
            return next != null;
        }

        public bool TryReadPrevious([NotNullWhen(true)] out IScoreMeasureReader? previous)
        {
            source.TryReadPrevious(out var _previous);
            previous = _previous?.Proxy();
            return previous != null;
        }

        public IInstrumentMeasureReader ReadMeasure(int ribbonIndex)
        {
            return source.GetMeasureCore(ribbonIndex).Proxy();
        }

        public IEnumerable<IInstrumentMeasureReader> ReadMeasures()
        {
            return source.EnumerateMeasuresCore().Select(e => e.Proxy());
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadMeasures();
        }
    }
}
