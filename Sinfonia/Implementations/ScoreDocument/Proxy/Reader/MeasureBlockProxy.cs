using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal class MeasureBlockReaderProxy : IMeasureBlockReader
    {
        private readonly MeasureBlock source;

        public MeasureBlockReaderProxy(MeasureBlock source)
        {
            this.source = source;
        }



        public bool Grace => source.Grace;

        public RythmicDuration RythmicDuration => source.RythmicDuration;

        public int Id => source.Id;
        public Guid Guid => source.Guid;

        public Position Position => source.Position;

        public Tuplet Tuplet => source.Tuplet;




        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadChords();
        }

        public IEnumerable<IChordReader> ReadChords()
        {
            return source.GetChordsCore().Select(e => e.Proxy());
        }

        public bool TryReadNext([NotNullWhen(true)] out IMeasureBlockReader? right)
        {
            right = null;
            if (source.TryReadNext(out MeasureBlock? _right))
            {
                right = _right.Proxy();
            }
            return right is not null;
        }

        public bool TryReadPrevious([NotNullWhen(true)] out IMeasureBlockReader? previous)
        {
            previous = null;
            if (source.TryReadNext(out MeasureBlock? _prev))
            {
                previous = _prev.Proxy();
            }
            return previous is not null;
        }
    }
}
