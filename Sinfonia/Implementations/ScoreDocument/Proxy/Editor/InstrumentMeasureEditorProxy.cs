using Sinfonia.Implementations.Commands;
using Sinfonia.Implementations.ScoreDocument.Layout;
using Sinfonia.Implementations.ScoreDocument.Memento.Layout;
using Sinfonia.Implementations.ScoreDocument.Proxy.Reader;
using StudioLaValse.ScoreDocument.Layout;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class InstrumentMeasureEditorProxy : IInstrumentMeasureEditor, IUniqueScoreElement
    {
        private readonly InstrumentMeasure source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;


        public int MeasureIndex => source.MeasureIndex;

        public int RibbonIndex => source.RibbonIndex;

        public TimeSignature TimeSignature => source.TimeSignature;

        public Instrument Instrument => source.Instrument;

        public Guid Guid => source.Guid;

        public int Id => source.Id;


        public InstrumentMeasureEditorProxy(InstrumentMeasure source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }




        public void AddVoice(int voice)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<InstrumentMeasure, InstrumentMeasureMemento>(source, s => s.AddVoice(voice)).ThenInvalidate(notifyEntityChanged, source.ProxyReader());
            transaction.Enqueue(command);
        }

        public void RemoveVoice(int voice)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<InstrumentMeasure, InstrumentMeasureMemento>(source, s => s.RemoveVoice(voice)).ThenInvalidate(notifyEntityChanged, source.ProxyReader());
            transaction.Enqueue(command);
        }

        public void Clear()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<InstrumentMeasure, InstrumentMeasureMemento>(source, s => s.Clear()).ThenInvalidate(notifyEntityChanged, source.ProxyReader());
            transaction.Enqueue(command);
        }

        public IMeasureBlockChainEditor ReadBlockChainAt(int voice)
        {
            return source.GetBlockChainOrThrowCore(voice).ProxyEditor(commandManager, notifyEntityChanged);
        }

        public bool TryReadPrevious([NotNullWhen(true)] out IInstrumentMeasureEditor? previous)
        {
            _ = source.TryReadPrevious(out var _previous);
            previous = _previous?.ProxyEditor(commandManager, notifyEntityChanged);
            return previous != null;
        }

        public bool TryReadNext([NotNullWhen(true)] out IInstrumentMeasureEditor? next)
        {
            _ = source.TryReadNext(out var _next);
            next = _next?.ProxyEditor(commandManager, notifyEntityChanged);
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

        public void AddClefChange(ClefChange clefChange)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutMementoCommand<InstrumentMeasureLayout, InstrumentMeasureLayoutMemento>(source.Layout, l => l.AddClefChange(clefChange)).ThenInvalidate(notifyEntityChanged, source.ProxyReader());
            transaction.Enqueue(command);
        }

        public void RemoveLayout()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RestoreLayoutCommand<InstrumentMeasureLayout, InstrumentMeasureLayoutMemento>(source.Layout).ThenInvalidate(notifyEntityChanged, source.ProxyReader());
            transaction.Enqueue(command);
        }

        public IInstrumentMeasureLayout ReadLayout()
        {
            return source.Layout;
        }
    }
}
