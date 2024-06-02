using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Implementation.Extensions;
using StudioLaValse.ScoreDocument.Implementation.Layout;
using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.Models.Base;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class InstrumentRibbonEditorProxy : IInstrumentRibbonEditor
    {
        private readonly InstrumentRibbon source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;



        public ScoreDocumentCore HostScoreDocument => source.HostScoreDocument;

        public int IndexInScore => source.IndexInScore;

        public Instrument Instrument => source.Instrument;


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
            return source.AuthorLayout;
        }

        public void RemoveLayout()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RestoreLayoutCommand<AuthorInstrumentRibbonLayout, InstrumentRibbonLayoutMembers>(source.AuthorLayout).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);
            transaction.Enqueue(command);
        }

        public void SetDisplayName(string name)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutMementoCommand<AuthorInstrumentRibbonLayout, InstrumentRibbonLayoutMembers>(source.AuthorLayout, l => l.DisplayName = name).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);
            transaction.Enqueue(command);
        }

        public void SetAbbreviatedName(string abbreviation)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<InstrumentRibbon, InstrumentRibbonModel>(source, s => s.AuthorLayout.AbbreviatedName = abbreviation).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);
            transaction.Enqueue(command);
        }

        public void SetNumberOfStaves(int numberOfStaves)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<InstrumentRibbon, InstrumentRibbonModel>(source, s => s.AuthorLayout.NumberOfStaves = numberOfStaves).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);
            transaction.Enqueue(command);
        }

        public void SetCollapsed(bool isCollapsed)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<InstrumentRibbon, InstrumentRibbonModel>(source, s => s.AuthorLayout.Collapsed = isCollapsed).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);
            transaction.Enqueue(command);
        }
    }
}
