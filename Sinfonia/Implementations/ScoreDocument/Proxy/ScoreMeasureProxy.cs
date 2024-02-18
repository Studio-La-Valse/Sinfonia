using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument.Proxy
{
    internal class ScoreMeasureProxy : IScoreMeasureEditor, IScoreMeasureReader
    {
        private readonly ScoreMeasure source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;

        public int IndexInScore => source.IndexInScore;

        public TimeSignature TimeSignature => source.TimeSignature;

        public bool IsLastInScore => source.IsLastInScore;

        public KeySignature KeySignature
        {
            get => source.KeySignature;
            set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<ScoreMeasure, ScoreMeasureMemento>(source, s => s.KeySignature = value);
                transaction.Enqueue(command);
            }
        }
        public double PaddingLeft
        {
            get => source.PaddingLeft;
            set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<ScoreMeasure, ScoreMeasureMemento>(source, s => s.PaddingLeft = value);
                transaction.Enqueue(command);
            }
        }
        public double PaddingRight
        {
            get => source.PaddingRight;
            set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<ScoreMeasure, ScoreMeasureMemento>(source, s => s.PaddingRight = value);
                transaction.Enqueue(command);
            }
        }
        public double Width
        {
            get => source.Width;
            set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<ScoreMeasure, ScoreMeasureMemento>(source, s => s.Width = value);
                transaction.Enqueue(command);
            }
        }
        public bool IsNewSystem
        {
            get => source.IsNewSystem;
            set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<ScoreMeasure, ScoreMeasureMemento>(source, s => s.IsNewSystem = value);
                transaction.Enqueue(command);
            }
        }

        public int Id => source.Id;

        public ScoreMeasureProxy(ScoreMeasure source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }





        public IInstrumentMeasureEditor EditMeasure(int ribbonIndex)
        {
            return source.GetMeasureCore(ribbonIndex).Proxy(commandManager, notifyEntityChanged);
        }

        public IEnumerable<IInstrumentMeasureEditor> EditMeasures()
        {
            return source.EnumerateMeasuresCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public IStaffSystemEditor EditStaffSystemOrigin()
        {
            return source.StaffSystemOrigin.Proxy(commandManager, notifyEntityChanged);
        }

        public IEnumerable<IInstrumentMeasure> EnumerateMeasures()
        {
            return source.EnumerateMeasuresCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public IStaffSystem GetStaffSystemOrigin()
        {
            return source.GetStaffSystemOrigin().Proxy(commandManager, notifyEntityChanged);
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
