using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument.Proxy
{
    internal class MeasureBlockEditorProxy : IMeasureBlockEditor, IMeasureBlockReader
    {
        private readonly MeasureBlock source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;

        public MeasureBlockEditorProxy(MeasureBlock source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }

        public bool Grace => source.Grace;

        public RythmicDuration RythmicDuration => source.RythmicDuration;

        public int Id => source.Id;

        public Position Position => source.Position;

        public Tuplet Tuplet => source.Tuplet;

        public double StemLength
        {
            get => source.StemLength; set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<MeasureBlock, MeasureBlockMemento>(source, (s) => s.StemLength = value);
                transaction.Enqueue(command);
            }
        }
        public double BeamAngle
        {
            get => source.BeamAngle; set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<MeasureBlock, MeasureBlockMemento>(source, (s) => s.BeamAngle = value);
                transaction.Enqueue(command);
            }
        }

        public void AppendChord(RythmicDuration rythmicDuration)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<MeasureBlock, MeasureBlockMemento>(source, (s) => s.AppendChord(rythmicDuration));
            transaction.Enqueue(command);
        }

        public void Splice(int index)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<MeasureBlock, MeasureBlockMemento>(source, (s) => s.Splice(index));
            transaction.Enqueue(command);
        }

        public IEnumerable<IChordEditor> EditChords()
        {
            return source.GetChordsCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public bool Equals(IUniqueScoreElement? other)
        {
            return source.Equals(other);
        }

        public void Rebeam()
        {
            source.Rebeam();
        }

        public void Clear()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<MeasureBlock, MeasureBlockMemento>(source, (s) => s.Clear());
            transaction.Enqueue(command);
        }

        public IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return source.EnumerateChildren();
        }

        public IEnumerable<IChordReader> ReadChords()
        {
            return source.GetChordsCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public bool TryReadNext([NotNullWhen(true)] out IMeasureBlockReader? right)
        {
            right = null;
            if (source.TryReadNext(out var _right))
            {
                right = _right.Proxy(commandManager, notifyEntityChanged);
            }
            return right is not null;
        }

        public bool TryReadPrevious([NotNullWhen(true)] out IMeasureBlockReader? previous)
        {
            previous = null;
            if (source.TryReadNext(out var _prev))
            {
                previous = _prev.Proxy(commandManager, notifyEntityChanged);
            }
            return previous is not null;
        }

        public IEnumerable<IChord> EnumerateChords()
        {
            return source.GetChordsCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }
    }
}
