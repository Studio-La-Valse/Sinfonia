using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Models;

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
            return source.SecondaryLayout;
        }

        public void RemoveLayout()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RestoreLayoutCommand<SecondaryInstrumentRibbonLayout, InstrumentRibbonLayoutModel>(source.SecondaryLayout).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);
            transaction.Enqueue(command);
        }

        public void SetDisplayName(string name)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutMementoCommand<SecondaryInstrumentRibbonLayout, InstrumentRibbonLayoutModel>(source.SecondaryLayout, l => l.DisplayName = name).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);
            transaction.Enqueue(command);
        }

        public void SetAbbreviatedName(string abbreviation)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<InstrumentRibbon, InstrumentRibbonModel>(source, s => s.Layout.AbbreviatedName = abbreviation).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);
            transaction.Enqueue(command);
        }

        public void SetNumberOfStaves(int numberOfStaves)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<InstrumentRibbon, InstrumentRibbonModel>(source, s => s.Layout.NumberOfStaves = numberOfStaves).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);
            transaction.Enqueue(command);
        }

        public void SetCollapsed(bool isCollapsed)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<InstrumentRibbon, InstrumentRibbonModel>(source, s => s.Layout.Collapsed = isCollapsed).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);
            transaction.Enqueue(command);
        }
    }
}
