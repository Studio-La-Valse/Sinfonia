using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Core;
using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Reader;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class InstrumentMeasureReaderProxy : IInstrumentMeasureReader
    {
        private readonly InstrumentMeasure source;



        public int MeasureIndex => source.MeasureIndex;

        public int RibbonIndex => source.RibbonIndex;

        public TimeSignature TimeSignature => source.TimeSignature;

        public Instrument Instrument => source.Instrument;

        public int Id => source.Id;




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
            _ = source.TryReadPrevious(out var _previous);
            previous = _previous?.ProxyReader();
            return previous != null;
        }

        public bool TryReadNext([NotNullWhen(true)] out IInstrumentMeasureReader? next)
        {
            _ = source.TryReadNext(out var _next);
            next = _next?.ProxyReader();
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

        public IInstrumentMeasureLayout ReadLayout()
        {
            return source.AuthorLayout;
        }

        public override string ToString()
        {
            return $"Instrument Measure : [{MeasureIndex}]";
        }
    }
}
