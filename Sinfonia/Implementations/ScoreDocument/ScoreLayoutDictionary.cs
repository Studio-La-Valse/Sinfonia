using Sinfonia.Implementations.Commands;
using Sinfonia.Implementations.ScoreDocument.Proxy.Editor;
using Sinfonia.Implementations.ScoreDocument.Proxy.Reader;
using StudioLaValse.ScoreDocument.Core.Primitives;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class ScoreLayoutDictionary : IScoreDocumentLayout
    {
        private readonly Dictionary<Guid, PageLayout> pageLayoutDictionary = [];

        private readonly Dictionary<Guid, NoteLayout> noteLayoutDictionary = [];
        private readonly Dictionary<Guid, ChordLayout> chordLayoutDictionary = [];
        private readonly Dictionary<Guid, MeasureBlockLayout> measureBlockLayoutDictionary = [];

        private readonly Dictionary<Guid, InstrumentMeasureLayout> instrumentMeasureLayoutDictionary = [];
        private readonly Dictionary<Guid, ScoreMeasureLayout> scoreMeasureLayoutDictionary = [];
        private readonly Dictionary<Guid, InstrumentRibbonLayout> instrumentRibbonLayoutDictionary = [];

        private readonly Dictionary<Guid, StaffLayout> staffLayoutDictionary = [];
        private readonly Dictionary<Guid, StaffGroupLayout> staffGroupLayoutDictionary = [];
        private readonly Dictionary<Guid, StaffSystemLayout> staffSystemLayoutDictionary = [];
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;
        private ScoreDocumentLayout? documentLayout;


        public ScoreDocumentStyleTemplate Template { get; }


        public ScoreLayoutDictionary(ScoreDocumentStyleTemplate documentStyle, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            Template = documentStyle;

            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }



        public void Apply(ScoreDocumentEditorProxy scoreDocumentCore, ScoreDocumentLayout layout)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            ScoreDocumentLayout? oldLayout = documentLayout?.Copy() ?? null;
            BaseCommand command = new SimpleCommand(() => { documentLayout = layout; }, () => { documentLayout = oldLayout; }).ThenInvalidate(notifyEntityChanged, scoreDocumentCore);
            transaction.Enqueue(command);
        }
        public void Restore(ScoreDocumentEditorProxy scoreDocumentCore)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            ScoreDocumentLayout? oldLayout = null;
            BaseCommand command = new SimpleCommand(
                () =>
                {
                    oldLayout = documentLayout;
                    if (oldLayout is not null)
                    {
                        documentLayout = null;
                    }
                },
                () =>
                {
                    documentLayout = oldLayout;
                }).ThenInvalidate(notifyEntityChanged, scoreDocumentCore);
            transaction.Enqueue(command);
        }

        public void Apply(PageEditorProxy page, PageLayout layout)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            BaseCommand command = new LayoutCommand<PageLayout>(pageLayoutDictionary, page.Guid, layout).ThenInvalidate(notifyEntityChanged, page.HostScoreDocument.Proxy());
            transaction.Enqueue(command);
        }
        public void Restore(PageEditorProxy page)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RemoveLayoutCommand<PageLayout>(pageLayoutDictionary, page.Guid).ThenInvalidate(notifyEntityChanged, page.HostScoreDocument.Proxy());
            transaction.Enqueue(command);
        }



        public void Apply(NoteEditorProxy note, NoteLayout layout)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            BaseCommand command = new LayoutCommand<NoteLayout>(noteLayoutDictionary, note.Guid, layout).ThenInvalidate(notifyEntityChanged, note.HostMeasure.Proxy());
            transaction.Enqueue(command);
        }
        public void Restore(NoteEditorProxy element)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            BaseCommand command = new RemoveLayoutCommand<NoteLayout>(noteLayoutDictionary, element.Guid).ThenInvalidate(notifyEntityChanged, element.HostMeasure.Proxy());
            transaction.Enqueue(command);
        }


        public void Apply(ChordEditorProxy chord, ChordLayout layout)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            BaseCommand command = new LayoutCommand<ChordLayout>(chordLayoutDictionary, chord.Guid, layout).ThenInvalidate(notifyEntityChanged, chord.HostMeasure.Proxy());
            transaction.Enqueue(command);
        }
        public void Restore(ChordEditorProxy element)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            BaseCommand command = new RemoveLayoutCommand<ChordLayout>(chordLayoutDictionary, element.Guid).ThenInvalidate(notifyEntityChanged, element.HostMeasure.Proxy());
            transaction.Enqueue(command);
        }


        public void Apply(MeasureBlockEditorProxy measureBlock, MeasureBlockLayout layout)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutCommand<MeasureBlockLayout>(measureBlockLayoutDictionary, measureBlock.Guid, layout).ThenInvalidate(notifyEntityChanged, measureBlock.HostMeasure.Proxy());
            transaction.Enqueue(command);
        }
        public void Restore(MeasureBlockEditorProxy element)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RemoveLayoutCommand<MeasureBlockLayout>(measureBlockLayoutDictionary, element.Guid).ThenInvalidate(notifyEntityChanged, element.HostMeasure.Proxy());
            transaction.Enqueue(command);
        }


        public void Apply(InstrumentMeasureEditorProxy instrumentMeasure, InstrumentMeasureLayout layout)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutCommand<InstrumentMeasureLayout>(instrumentMeasureLayoutDictionary, instrumentMeasure.Guid, layout).ThenInvalidate(notifyEntityChanged, instrumentMeasure);
            transaction.Enqueue(command);
        }
        public void Restore(InstrumentMeasureEditorProxy element)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RemoveLayoutCommand<InstrumentMeasureLayout>(instrumentMeasureLayoutDictionary, element.Guid).ThenInvalidate(notifyEntityChanged, element);
            transaction.Enqueue(command);
        }



        public void Apply(ScoreMeasureEditorProxy scoreMeasure, ScoreMeasureLayout layout)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutCommand<ScoreMeasureLayout>(scoreMeasureLayoutDictionary, scoreMeasure.Guid, layout).ThenInvalidate(notifyEntityChanged, scoreMeasure);
            transaction.Enqueue(command);
        }
        public void Restore(ScoreMeasureEditorProxy element)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RemoveLayoutCommand<ScoreMeasureLayout>(scoreMeasureLayoutDictionary, element.Guid).ThenInvalidate(notifyEntityChanged, element);
            transaction.Enqueue(command);
        }



        public void Apply(InstrumentRibbonEditorProxy instrumentRibbon, InstrumentRibbonLayout layout)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutCommand<InstrumentRibbonLayout>(instrumentRibbonLayoutDictionary, instrumentRibbon.Guid, layout).ThenInvalidate(notifyEntityChanged, instrumentRibbon.HostScoreDocument.Proxy());
            transaction.Enqueue(command);
        }
        public void Restore(InstrumentRibbonEditorProxy element)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RemoveLayoutCommand<InstrumentRibbonLayout>(instrumentRibbonLayoutDictionary, element.Guid).ThenInvalidate(notifyEntityChanged, element.HostScoreDocument.Proxy());
            transaction.Enqueue(command);
        }



        public void Apply(StaffGroupEditorProxy staffGroup, StaffGroupLayout layout)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutCommand<StaffGroupLayout>(staffGroupLayoutDictionary, staffGroup.Guid, layout).ThenInvalidate(notifyEntityChanged, staffGroup.HostScoreDocument.Proxy());
            transaction.Enqueue(command);
        }
        public void Restore(StaffGroupEditorProxy element)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RemoveLayoutCommand<StaffGroupLayout>(staffGroupLayoutDictionary, element.Guid).ThenInvalidate(notifyEntityChanged, element.HostScoreDocument.Proxy());
            transaction.Enqueue(command);
        }



        public void Apply(StaffEditorProxy staff, StaffLayout layout)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutCommand<StaffLayout>(staffLayoutDictionary, staff.Guid, layout).ThenInvalidate(notifyEntityChanged, staff.HostScoreDocument.Proxy());
            transaction.Enqueue(command);
        }
        public void Restore(StaffEditorProxy element)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RemoveLayoutCommand<StaffLayout>(staffLayoutDictionary, element.Guid).ThenInvalidate(notifyEntityChanged, element.HostScoreDocument.Proxy());
            transaction.Enqueue(command);
        }


        public void Apply(StaffSystemEditorProxy staffSystem, StaffSystemLayout layout)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutCommand<StaffSystemLayout>(staffSystemLayoutDictionary, staffSystem.Guid, layout).ThenInvalidate(notifyEntityChanged, staffSystem.HostScoreDocument.Proxy());
            transaction.Enqueue(command);
        }
        public void Restore(StaffSystemEditorProxy element)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RemoveLayoutCommand<StaffSystemLayout>(staffSystemLayoutDictionary, element.Guid).ThenInvalidate(notifyEntityChanged, element.HostScoreDocument.Proxy());
            transaction.Enqueue(command);
        }


        public void Clean()
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            transaction.Enqueue(new WipeDictionaryCommand<ChordLayout>(chordLayoutDictionary));
            transaction.Enqueue(new WipeDictionaryCommand<PageLayout>(pageLayoutDictionary));
            transaction.Enqueue(new WipeDictionaryCommand<InstrumentMeasureLayout>(instrumentMeasureLayoutDictionary));
            transaction.Enqueue(new WipeDictionaryCommand<InstrumentRibbonLayout>(instrumentRibbonLayoutDictionary));
            transaction.Enqueue(new WipeDictionaryCommand<MeasureBlockLayout>(measureBlockLayoutDictionary));
            transaction.Enqueue(new WipeDictionaryCommand<NoteLayout>(noteLayoutDictionary));
            transaction.Enqueue(new WipeDictionaryCommand<ScoreMeasureLayout>(scoreMeasureLayoutDictionary));
            transaction.Enqueue(new WipeDictionaryCommand<StaffLayout>(staffLayoutDictionary));
            transaction.Enqueue(new WipeDictionaryCommand<StaffGroupLayout>(staffGroupLayoutDictionary));
            transaction.Enqueue(new WipeDictionaryCommand<StaffSystemLayout>(staffSystemLayoutDictionary));
        }



        public ScoreDocumentLayout DocumentLayout<TElement>(TElement element) where TElement : IScoreDocument, IScoreEntity
        {
            if (documentLayout is not null)
            {
                return documentLayout.Copy();
            }

            var newLayout = new ScoreDocumentLayout(Template);
            documentLayout = newLayout;
            return DocumentLayout(element);
        }
        public PageLayout PageLayout<TElement>(TElement element) where TElement : IPage, IScoreEntity
        {
            if (pageLayoutDictionary.TryGetValue(element.Guid, out PageLayout? value))
            {
                return value.Copy();
            }

            pageLayoutDictionary[element.Guid] = new PageLayout(Template.PageStyleTemplate);
            return PageLayout(element);
        }
        public InstrumentMeasureLayout InstrumentMeasureLayout<TElement>(TElement element) where TElement : IInstrumentMeasure, IScoreEntity
        {
            if (instrumentMeasureLayoutDictionary.TryGetValue(element.Guid, out InstrumentMeasureLayout? value))
            {
                return value.Copy();
            }

            instrumentMeasureLayoutDictionary[element.Guid] = new InstrumentMeasureLayout(Template.InstrumentMeasureStyleTemplate);
            return InstrumentMeasureLayout(element);
        }
        public InstrumentRibbonLayout InstrumentRibbonLayout<TElement>(TElement element) where TElement : IInstrumentRibbon, IScoreEntity
        {
            if (instrumentRibbonLayoutDictionary.TryGetValue(element.Guid, out InstrumentRibbonLayout? value))
            {
                return value.Copy();
            }

            instrumentRibbonLayoutDictionary[element.Guid] = new InstrumentRibbonLayout(Template.InstrumentRibbonStyleTemplate, element);
            return InstrumentRibbonLayout(element);
        }
        public MeasureBlockLayout MeasureBlockLayout<TElement>(TElement element) where TElement : IMeasureBlock, IScoreEntity
        {
            if (measureBlockLayoutDictionary.TryGetValue(element.Guid, out MeasureBlockLayout? value))
            {
                return value.Copy();
            }

            measureBlockLayoutDictionary[element.Guid] = new MeasureBlockLayout(Template.MeasureBlockStyleTemplate);
            return MeasureBlockLayout(element);
        }
        public ChordLayout ChordLayout<TElement>(TElement element) where TElement : IChord, IScoreEntity
        {
            if (chordLayoutDictionary.TryGetValue(element.Guid, out ChordLayout? value))
            {
                return value.Copy();
            }

            chordLayoutDictionary[element.Guid] = new ChordLayout(Template.ChordStyleTemplate);
            return ChordLayout(element);
        }
        public NoteLayout NoteLayout<TElement>(TElement element) where TElement : INote, IScoreEntity
        {
            if (noteLayoutDictionary.TryGetValue(element.Guid, out NoteLayout? value))
            {
                return value.Copy();
            }

            noteLayoutDictionary[element.Guid] = new NoteLayout(Template.NoteStyleTemplate, element.Grace);
            return NoteLayout(element);
        }
        public ScoreMeasureLayout ScoreMeasureLayout<TElement>(TElement element) where TElement : IScoreMeasure, IScoreEntity
        {
            if (scoreMeasureLayoutDictionary.TryGetValue(element.Guid, out ScoreMeasureLayout? value))
            {
                return value.Copy();
            }

            scoreMeasureLayoutDictionary[element.Guid] = new ScoreMeasureLayout(Template.ScoreMeasureStyleTemplate, element);
            return ScoreMeasureLayout(element);
        }
        public StaffGroupLayout StaffGroupLayout<TElement>(TElement element) where TElement : IStaffGroup, IScoreEntity
        {
            if (staffGroupLayoutDictionary.TryGetValue(element.Guid, out StaffGroupLayout? layout))
            {
                return layout.Copy();
            }

            staffGroupLayoutDictionary[element.Guid] = new StaffGroupLayout(Template.StaffGroupStyleTemplate, element.Instrument);
            return StaffGroupLayout(element);
        }
        public StaffLayout StaffLayout<TElement>(TElement element) where TElement : IStaff, IScoreEntity
        {
            if (staffLayoutDictionary.TryGetValue(element.Guid, out StaffLayout? layout))
            {
                return layout.Copy();
            }

            staffLayoutDictionary[element.Guid] = new StaffLayout(Template.StaffStyleTemplate);
            return StaffLayout(element);
        }
        public StaffSystemLayout StaffSystemLayout<TElement>(TElement element) where TElement : IStaffSystem, IScoreEntity
        {
            if (staffSystemLayoutDictionary.TryGetValue(element.Guid, out StaffSystemLayout? layout))
            {
                return layout.Copy();
            }

            staffSystemLayoutDictionary[element.Guid] = new StaffSystemLayout(Template.StaffSystemStyleTemplate);
            return StaffSystemLayout(element);
        }
    }
}