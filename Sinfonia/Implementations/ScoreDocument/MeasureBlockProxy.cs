using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Reader;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class MeasureBlockReaderProxy : IMeasureBlockReader
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
            return source.GetChordsCore().Select(e => e.ProxyReader());
        }

        public bool TryReadNext([NotNullWhen(true)] out IMeasureBlockReader? right)
        {
            right = null;
            if (source.TryReadNext(out var _right))
            {
                right = _right.ProxyReader();
            }
            return right is not null;
        }

        public bool TryReadPrevious([NotNullWhen(true)] out IMeasureBlockReader? previous)
        {
            previous = null;
            if (source.TryReadNext(out var _prev))
            {
                previous = _prev.ProxyReader();
            }
            return previous is not null;
        }


        public override string ToString()
        {
            return $"Measure Block : [{Guid}]";
        }

        public IMeasureBlockLayout ReadLayout()
        {
            return source.AuthorLayout;
        }
    }
}
