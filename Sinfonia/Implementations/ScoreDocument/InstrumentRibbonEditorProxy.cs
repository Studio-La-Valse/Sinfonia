using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Implementation.Extensions;
using StudioLaValse.ScoreDocument.Implementation.Layout;
using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.Models.Base;
using YamlDotNet.Core.Tokens;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class InstrumentRibbonEditorProxy : IInstrumentRibbon
    {
        private readonly InstrumentRibbon source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;



        public int IndexInScore => source.IndexInScore;

        public Instrument Instrument => source.Instrument;

        public int Id => source.Id;

        public AuthorInstrumentRibbonLayout AuthorLayout => source.AuthorLayout;

        public TemplateProperty<string> DisplayName => AuthorLayout.DisplayName.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);

        public TemplateProperty<string> AbbreviatedName => AuthorLayout.DisplayName.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);

        public TemplateProperty<bool> Collapsed => AuthorLayout.Collapsed.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);

        public TemplateProperty<int> NumberOfStaves => AuthorLayout.NumberOfStaves.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);

        public TemplateProperty<double> Scale => AuthorLayout.Scale.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);



        public InstrumentRibbonEditorProxy(InstrumentRibbon source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }




        public IInstrumentMeasure ReadMeasure(int measureIndex)
        {
            return source.GetMeasureCore(measureIndex).ProxyEditor(commandManager, notifyEntityChanged);
        }

        public IEnumerable<IInstrumentMeasure> ReadMeasures()
        {
            return source.EnumerateMeasuresCore().Select(e => e.ProxyEditor(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadMeasures();
        }

        public void Restore()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RestoreLayoutCommand<AuthorInstrumentRibbonLayout, InstrumentRibbonLayoutMembers>(AuthorLayout).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);
            transaction.Enqueue(command);
        }

        public void ResetDisplayName()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutMementoCommand<AuthorInstrumentRibbonLayout, InstrumentRibbonLayoutMembers>(AuthorLayout, s => s.ResetDisplayName()).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);
            transaction.Enqueue(command);
        }

        public void ResetAbbreviatedName()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutMementoCommand<AuthorInstrumentRibbonLayout, InstrumentRibbonLayoutMembers>(AuthorLayout, s => s.ResetAbbreviatedName()).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);
            transaction.Enqueue(command);
        }

        public void ResetCollapsed()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutMementoCommand<AuthorInstrumentRibbonLayout, InstrumentRibbonLayoutMembers>(AuthorLayout, s => s.ResetCollapsed()).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);
            transaction.Enqueue(command);
        }

        public void ResetNumberOfStaves()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutMementoCommand<AuthorInstrumentRibbonLayout, InstrumentRibbonLayoutMembers>(AuthorLayout, s => s.ResetNumberOfStaves()).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);
            transaction.Enqueue(command);
        }

        public void ResetScale()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutMementoCommand<AuthorInstrumentRibbonLayout, InstrumentRibbonLayoutMembers>(AuthorLayout, s => s.ResetScale()).ThenInvalidate(notifyEntityChanged, source.HostScoreDocument);
            transaction.Enqueue(command);
        }

        public bool Equals(IUniqueScoreElement? other)
        {
            return other is not null && other.Id == Id;
        }
    }
}
