using Sinfonia.Extensions;
using Sinfonia.Implementations.Commands;
using Sinfonia.Implementations.ScoreDocument.Layout;
using Sinfonia.Implementations.ScoreDocument.Memento.Layout;
using Sinfonia.Implementations.ScoreDocument.Proxy.Reader;
using StudioLaValse.ScoreDocument.Layout;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class InstrumentRibbonEditorProxy : IInstrumentRibbonEditor
    {
        private readonly InstrumentRibbon source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;



        public ScoreDocumentCore HostScoreDocument => source.HostScoreDocument;

        public int IndexInScore => source.IndexInScore;

        public Instrument Instrument => source.Instrument;

        public Guid Guid => source.Guid;


        public InstrumentRibbonEditorProxy(InstrumentRibbon source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }




        public IInstrumentMeasureEditor ReadMeasure(int measureIndex)
        {
            return source.GetMeasureCore(measureIndex).ProxyEditor(commandManager, notifyEntityChanged);
        }

        public IEnumerable<IInstrumentMeasureEditor> ReadMeasures()
        {
            return source.EnumerateMeasuresCore().Select(e => e.ProxyEditor(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadMeasures();
        }

        public IInstrumentRibbonLayout ReadLayout()
        {
            return source.Layout;
        }

        public void RemoveLayout()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RestoreLayoutCommand<InstrumentRibbonLayout, InstrumentRibbonLayoutMemento>(source.Layout).ThenInvalidate(notifyEntityChanged, source.ProxyReader());
            transaction.Enqueue(command);
        }

        public void SetDisplayName(string name)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutMementoCommand<InstrumentRibbonLayout, InstrumentRibbonLayoutMemento>(source.Layout, l => l.DisplayName = name).ThenInvalidate(notifyEntityChanged, source.ProxyReader());
            transaction.Enqueue(command);
        }
    }
}
