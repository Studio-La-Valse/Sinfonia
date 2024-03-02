using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class InstrumentMeasureEditorProxy : IInstrumentMeasureEditor
    {
        private readonly InstrumentMeasure source;
        private readonly ScoreLayoutDictionary scoreLayoutDictionary;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;


        public int MeasureIndex => source.MeasureIndex;

        public int RibbonIndex => source.RibbonIndex;

        public TimeSignature TimeSignature => source.TimeSignature;

        public Instrument Instrument => source.Instrument;

        public KeySignature KeySignature => source.KeySignature;

        public Guid Guid => source.Guid;

        public int Id => source.Id;


        public InstrumentMeasureEditorProxy(InstrumentMeasure source, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.scoreLayoutDictionary = scoreLayoutDictionary;
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

        public void Clear()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<InstrumentMeasure, InstrumentMeasureMemento>(source, s => s.Clear());
            transaction.Enqueue(command);
        }

        public IMeasureBlockChainEditor ReadBlockChainAt(int voice)
        {
            return source.GetBlockChainOrThrowCore(voice).ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged);
        }

        public bool TryReadPrevious([NotNullWhen(true)] out IInstrumentMeasureEditor? previous)
        {
            source.TryReadPrevious(out var _previous);
            previous = _previous?.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged);
            return previous != null;
        }

        public bool TryReadNext([NotNullWhen(true)] out IInstrumentMeasureEditor? next)
        {
            source.TryReadNext(out var _next);
            next = _next?.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged);
            return next != null;
        }

        public IEnumerable<int> ReadVoices()
        {
            return source.EnumerateVoices();
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadVoices().Select(v => ReadBlockChainAt(v));
        }
    }
}
