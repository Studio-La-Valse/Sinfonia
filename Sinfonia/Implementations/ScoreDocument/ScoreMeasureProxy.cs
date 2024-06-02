using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Primitives;
using StudioLaValse.ScoreDocument.Reader;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class ScoreMeasureReaderProxy : IScoreMeasureReader
    {
        private readonly ScoreMeasure source;



        public int IndexInScore => source.IndexInScore;

        public Guid Guid => source.Guid;

        public TimeSignature TimeSignature => source.TimeSignature;

        public bool IsLastInScore => source.IsLastInScore;

        public int Id => source.Id;


        public ScoreMeasureReaderProxy(ScoreMeasure source)
        {
            this.source = source;
        }



        public bool TryReadNext([NotNullWhen(true)] out IScoreMeasureReader? next)
        {
            _ = source.TryReadNext(out var _next);
            next = _next?.ProxyReader();
            return next != null;
        }

        public bool TryReadPrevious([NotNullWhen(true)] out IScoreMeasureReader? previous)
        {
            _ = source.TryReadPrevious(out var _previous);
            previous = _previous?.ProxyReader();
            return previous != null;
        }

        public IInstrumentMeasureReader ReadMeasure(int ribbonIndex)
        {
            return source.GetMeasureCore(ribbonIndex).ProxyReader();
        }

        public IEnumerable<IInstrumentMeasureReader> ReadMeasures()
        {
            return source.EnumerateMeasuresCore().Select(e => e.ProxyReader());
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadMeasures();
        }


        public override string ToString()
        {
            return $"Score Measure : [{Guid}]";
        }

        public IScoreMeasureLayout ReadLayout()
        {
            return source.AuthorLayout;
        }
    }
}
