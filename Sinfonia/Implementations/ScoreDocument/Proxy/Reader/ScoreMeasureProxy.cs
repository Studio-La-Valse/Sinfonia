using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class ScoreMeasureReaderProxy : IScoreMeasureReader
    {
        private readonly ScoreMeasure source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;

        public int IndexInScore => source.IndexInScore;

        public Guid Guid => source.Guid;

        public TimeSignature TimeSignature => source.TimeSignature;

        public bool IsLastInScore => source.IsLastInScore;

        public int Id => source.Id;

        public KeySignature KeySignature
        {
            get => source.KeySignature;
            set => source.KeySignature = value;
        }



        public ScoreMeasureReaderProxy(ScoreMeasure source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }





        public IInstrumentMeasureReader ReadMeasure(int ribbonIndex)
        {
            return source.GetMeasureCore(ribbonIndex).Proxy(commandManager, notifyEntityChanged);
        }

        public IEnumerable<IInstrumentMeasureReader> ReadMeasures()
        {
            return source.EnumerateMeasuresCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public bool TryReadNext([NotNullWhen(true)] out IScoreMeasureReader? next)
        {
            source.TryReadNext(out var _next);
            next = _next?.Proxy(commandManager, notifyEntityChanged);
            return next != null;
        }

        public bool TryReadPrevious([NotNullWhen(true)] out IScoreMeasureReader? previous)
        {
            source.TryReadPrevious(out var _previous);
            previous = _previous?.Proxy(commandManager, notifyEntityChanged);
            return previous != null;
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
