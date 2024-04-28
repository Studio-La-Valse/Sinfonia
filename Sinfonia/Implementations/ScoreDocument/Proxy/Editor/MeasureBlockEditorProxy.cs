using Sinfonia.Extensions;
using Sinfonia.Implementations.Commands;
using Sinfonia.Implementations.ScoreDocument.Layout;
using Sinfonia.Implementations.ScoreDocument.Memento.Layout;
using Sinfonia.Implementations.ScoreDocument.Proxy.Reader;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class MeasureBlockEditorProxy(MeasureBlock source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged) : IMeasureBlockEditor, IUniqueScoreElement
    {
        private readonly MeasureBlock source = source;
        private readonly ICommandManager commandManager = commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = notifyEntityChanged;


        public bool Grace => source.Grace;

        public RythmicDuration RythmicDuration => source.RythmicDuration;

        public Position Position => source.Position;

        public Tuplet Tuplet => source.Tuplet;

        public Guid Guid => source.Guid;

        public int Id => source.Id;

        public InstrumentMeasure HostMeasure => source.RibbonMeasure;


        public void AppendChord(RythmicDuration rythmicDuration)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            MementoCommand<MeasureBlock, MeasureBlockMemento> command = new(source, (s) => s.AppendChord(rythmicDuration));
            transaction.Enqueue(command);
        }

        public void Splice(int index)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            MementoCommand<MeasureBlock, MeasureBlockMemento> command = new(source, (s) => s.Splice(index));
            transaction.Enqueue(command);
        }

        public void Clear()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            MementoCommand<MeasureBlock, MeasureBlockMemento> command = new(source, (s) => s.Clear());
            transaction.Enqueue(command);
        }

        public bool TryReadNext([NotNullWhen(true)] out IMeasureBlockEditor? right)
        {
            right = null;
            if (source.TryReadNext(out var _right))
            {
                right = _right.ProxyEditor(commandManager, notifyEntityChanged);
            }
            return right is not null;
        }

        public bool TryReadPrevious([NotNullWhen(true)] out IMeasureBlockEditor? previous)
        {
            previous = null;
            if (source.TryReadNext(out var _prev))
            {
                previous = _prev.ProxyEditor(commandManager, notifyEntityChanged);
            }
            return previous is not null;
        }

        public IEnumerable<IChordEditor> ReadChords()
        {
            return source.GetChordsCore().Select(e => e.ProxyEditor(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadChords();
        }

        public IMeasureBlockLayout ReadLayout()
        {
            return source.Layout;
        }

        public void RemoveLayout()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RestoreLayoutCommand<MeasureBlockLayout, MeasureBlockLayoutMemento>(source.Layout).ThenInvalidate(notifyEntityChanged, source.ProxyReader());
            transaction.Enqueue(command);
        }
    }
}
