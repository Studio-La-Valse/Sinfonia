using Sinfonia.Implementations.ScoreDocument.Proxy.Editor;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal class InstrumentMeasureReaderProxy : IInstrumentMeasureReader
    {
        private readonly InstrumentMeasure source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;


        public int MeasureIndex =>
            source.MeasureIndex;
        public int RibbonIndex =>
            source.RibbonIndex;
        public TimeSignature TimeSignature =>
            source.TimeSignature;
        public Instrument Instrument =>
            source.Instrument;
        public int Id =>
            source.Id;
        public Guid Guid =>
            source.Guid;
        public KeySignature KeySignature => 
            source.ReadMeasureContext().KeySignature;

        public InstrumentMeasureReaderProxy(InstrumentMeasure source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }




        public void AddVoice(int voice)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<InstrumentMeasure, InstrumentMeasureMemento>(source, s => s.AddVoice(voice));
            transaction.Enqueue(command);
        }

        public void RemoveVoice(int voice)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<InstrumentMeasure, InstrumentMeasureMemento>(source, s => s.RemoveVoice(voice));
            transaction.Enqueue(command);
        }

        public bool Equals(IUniqueScoreElement? other)
        {
            return source.Equals(other);
        }

        public void Clear()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<InstrumentMeasure, InstrumentMeasureMemento>(source, s => s.Clear());
            transaction.Enqueue(command);
        }

        public IMeasureBlockChainReader ReadBlockChainAt(int voice)
        {
            return source.GetBlockChainOrThrowCore(voice).Proxy(commandManager, notifyEntityChanged);
        }

        public IScoreMeasureReader ReadMeasureContext()
        {
            return source.ReadMeasureContext().Proxy(commandManager, notifyEntityChanged);
        }

        public bool TryReadPrevious([NotNullWhen(true)] out IInstrumentMeasureReader? previous)
        {
            source.TryReadPrevious(out var _previous);
            previous = _previous?.Proxy(commandManager, notifyEntityChanged);
            return previous != null;
        }

        public bool TryReadNext([NotNullWhen(true)] out IInstrumentMeasureReader? next)
        {
            source.TryReadNext(out var _next);
            next = _next?.Proxy(commandManager, notifyEntityChanged);
            return next != null;
        }

        public IEnumerable<int> ReadVoices()
        {
            return source.EnumerateVoices();
        }

        public IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return source.EnumerateChildren();
        }
    }
}
