using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Implementation.Layout;
using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.Models.Base;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class InstrumentMeasureEditorProxy : IInstrumentMeasure
    {
        private readonly InstrumentMeasure source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;


        public IInstrumentMeasureLayout AuthoLayout => source.AuthorLayout;

        public int MeasureIndex => source.MeasureIndex;

        public int RibbonIndex => source.RibbonIndex;

        public TimeSignature TimeSignature => source.TimeSignature;

        public Instrument Instrument => source.Instrument;

        public int Id => source.Id;

        public ReadonlyTemplateProperty<KeySignature> KeySignature => AuthoLayout.KeySignature;

        public TemplateProperty<double?> PaddingBottom => AuthoLayout.PaddingBottom.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, source.HostMeasure.HostDocument);

        public TemplateProperty<bool?> Collapsed => AuthoLayout.Collapsed.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, source.HostMeasure.HostDocument);

        public TemplateProperty<int?> NumberOfStaves => AuthoLayout.NumberOfStaves.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, source.HostMeasure.HostDocument);



        public InstrumentMeasureEditorProxy(InstrumentMeasure source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }



        public IEnumerable<ClefChange> EnumerateClefChanges() => AuthoLayout.EnumerateClefChanges();

        public void AddVoice(int voice)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<InstrumentMeasure, InstrumentMeasureModel>(source, s => s.AddVoice(voice)).ThenInvalidate(notifyEntityChanged, source);
            transaction.Enqueue(command);
        }

        public void RemoveVoice(int voice)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<InstrumentMeasure, InstrumentMeasureModel>(source, s => s.RemoveVoice(voice)).ThenInvalidate(notifyEntityChanged, source);
            transaction.Enqueue(command);
        }

        public void Clear()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<InstrumentMeasure, InstrumentMeasureModel>(source, s => s.Clear()).ThenInvalidate(notifyEntityChanged, source);
            transaction.Enqueue(command);
        }

        public void AddClefChange(ClefChange clefChange)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutMementoCommand<AuthorInstrumentMeasureLayout, InstrumentMeasureLayoutMembers>(source.AuthorLayout, l => l.AddClefChange(clefChange)).ThenInvalidate(notifyEntityChanged, source);
            transaction.Enqueue(command);
        }

        public void RemoveClefChange(ClefChange clefChange)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutMementoCommand<AuthorInstrumentMeasureLayout, InstrumentMeasureLayoutMembers>(source.AuthorLayout, l => l.RemoveClefChange(clefChange)).ThenInvalidate(notifyEntityChanged, source);
            transaction.Enqueue(command);
        }

        public void RequestPaddingBottom(int staffIndex, double? paddingBottom = null)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutMementoCommand<AuthorInstrumentMeasureLayout, InstrumentMeasureLayoutMembers>(source.AuthorLayout, l => l.PaddingBottom.Value = paddingBottom).ThenInvalidate(notifyEntityChanged, source);
            transaction.Enqueue(command);
        }

        public void ClearClefChanges()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutMementoCommand<AuthorInstrumentMeasureLayout, InstrumentMeasureLayoutMembers>(source.AuthorLayout, l => l.ClearClefChanges()).ThenInvalidate(notifyEntityChanged, source);
            transaction.Enqueue(command);
        }

        public void Restore()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RestoreLayoutCommand<AuthorInstrumentMeasureLayout, InstrumentMeasureLayoutMembers>(source.AuthorLayout).ThenInvalidate(notifyEntityChanged, source);
            transaction.Enqueue(command);
        }

        public IMeasureBlockChain ReadBlockChainAt(int voice)
        {
            return source.GetBlockChainOrThrowCore(voice).ProxyEditor(commandManager, notifyEntityChanged);
        }

        public bool TryReadPrevious([NotNullWhen(true)] out IInstrumentMeasure? previous)
        {
            _ = source.TryReadPrevious(out var _previous);
            previous = _previous?.ProxyEditor(commandManager, notifyEntityChanged);
            return previous != null;
        }

        public bool TryReadNext([NotNullWhen(true)] out IInstrumentMeasure? next)
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
            return ReadVoices().Select(ReadBlockChainAt).SelectMany(e => e.ReadBlocks());
        }

        public double? GetPaddingBottom(int staffIndex)
        {
            return AuthoLayout.GetPaddingBottom(staffIndex);
        }

        public bool Equals(IUniqueScoreElement? other)
        {
            return other is not null && other.Id == Id;
        }
    }
}
