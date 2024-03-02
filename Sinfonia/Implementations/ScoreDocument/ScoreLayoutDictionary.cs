namespace Sinfonia.Implementations.ScoreDocument
{
    internal class ScoreLayoutDictionary : IScoreLayoutBuilder
    {
        private readonly Dictionary<Guid, NoteLayout> noteLayoutDictionary = [];
        private readonly Dictionary<Guid, ChordLayout> chordLayoutDictionary = [];
        private readonly Dictionary<Guid, MeasureBlockLayout> measureBlockLayoutDictionary = [];

        private readonly Dictionary<Guid, InstrumentMeasureLayout> instrumentMeasureLayoutDictionary = [];
        private readonly Dictionary<Guid, ScoreMeasureLayout> scoreMeasureLayoutDictionary = [];
        private readonly Dictionary<Guid, InstrumentRibbonLayout> instrumentRibbonLayoutDictionary = [];

        private readonly Dictionary<Guid, StaffLayout> staffLayoutDictionary = [];
        private readonly Dictionary<Guid, StaffGroupLayout> staffGroupLayoutDictionary = [];
        private readonly Dictionary<Guid, StaffSystemLayout> staffSystemLayoutDictionary = [];
        private readonly ScoreDocumentStyle documentStyle;
        private readonly ICommandManager commandManager;
        private ScoreDocumentLayout? documentLayout;


        public ScoreLayoutDictionary(ScoreDocumentStyle documentStyle, ICommandManager commandManager)
        {
            this.documentStyle = documentStyle;
            this.commandManager = commandManager;
        }



        public void Apply(ScoreDocumentLayout layout)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var oldLayout = documentLayout?.Copy() ?? null;
            var command = new SimpleCommand(
                () =>
                {
                    documentLayout = layout;
                },
                () =>
                {
                    documentLayout = oldLayout;
                });
            transaction.Enqueue(command);
        }
        public void Apply(INoteEditor note, NoteLayout layout)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutCommand<NoteLayout>(noteLayoutDictionary, note.Guid, layout);
            transaction.Enqueue(command);
        }
        public void Apply(IChordEditor chord, ChordLayout layout)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutCommand<ChordLayout>(chordLayoutDictionary, chord.Guid, layout);
            transaction.Enqueue(command);
        }
        public void Apply(IMeasureBlockEditor measureBlock, MeasureBlockLayout layout)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutCommand<MeasureBlockLayout>(measureBlockLayoutDictionary, measureBlock.Guid, layout);
            transaction.Enqueue(command);
        }
        public void Apply(IInstrumentMeasureEditor instrumentMeasure, InstrumentMeasureLayout layout)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutCommand<InstrumentMeasureLayout>(instrumentMeasureLayoutDictionary, instrumentMeasure.Guid, layout);
            transaction.Enqueue(command);
        }
        public void Apply(IScoreMeasureEditor scoreMeasure, ScoreMeasureLayout layout)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutCommand<ScoreMeasureLayout>(scoreMeasureLayoutDictionary, scoreMeasure.Guid, layout);
            transaction.Enqueue(command);
        }
        public void Apply(IInstrumentRibbonEditor instrumentRibbon, InstrumentRibbonLayout layout)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutCommand<InstrumentRibbonLayout>(instrumentRibbonLayoutDictionary, instrumentRibbon.Guid, layout);
            transaction.Enqueue(command);
        }
        public void Apply(IStaffGroupEditor staffGroup, StaffGroupLayout layout)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutCommand<StaffGroupLayout>(staffGroupLayoutDictionary, staffGroup.Guid, layout);
            transaction.Enqueue(command);
        }
        public void Apply(IStaffEditor staff, StaffLayout layout)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutCommand<StaffLayout>(staffLayoutDictionary, staff.Guid, layout);
            transaction.Enqueue(command);
        }
        public void Apply(IStaffSystemEditor staffSystem, StaffSystemLayout layout)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new LayoutCommand<StaffSystemLayout>(staffSystemLayoutDictionary, staffSystem.Guid, layout);
            transaction.Enqueue(command);
        }




        public ScoreDocumentLayout DocumentLayout<TElement>(TElement element) where TElement : IScoreDocument, IScoreEntity
        {
            if (documentLayout is null)
            {
                return documentStyle.ScoreDocumentLayoutFactory(element);
            }

            return documentLayout.Copy();
        }
        public InstrumentMeasureLayout InstrumentMeasureLayout<TElement>(TElement element) where TElement : IInstrumentMeasure, IScoreEntity
        {
            if (instrumentMeasureLayoutDictionary.TryGetValue(element.Guid, out var value))
            {
                return value.Copy();
            }

            return documentStyle.InstrumentMeasureLayoutFactory(element);
        }
        public InstrumentRibbonLayout InstrumentRibbonLayout<TElement>(TElement element) where TElement : IInstrumentRibbon, IScoreEntity
        {
            if (instrumentRibbonLayoutDictionary.TryGetValue(element.Guid, out var value))
            {
                return value.Copy();
            }

            return documentStyle.InstrumentRibbonLayoutFactory(element);
        }
        public MeasureBlockLayout MeasureBlockLayout<TElement>(TElement element) where TElement : IMeasureBlock, IScoreEntity
        {
            if (measureBlockLayoutDictionary.TryGetValue(element.Guid, out var value))
            {
                return value.Copy();
            }

            return documentStyle.MeasureBlockStyle(element);
        }
        public ChordLayout ChordLayout<TElement>(TElement element) where TElement : IChord, IScoreEntity
        {
            if (chordLayoutDictionary.TryGetValue(element.Guid, out var value))
            {
                return value.Copy();
            }

            return documentStyle.ChordLayoutFactory(element);
        }
        public NoteLayout NoteLayout<TElement>(TElement element) where TElement : INote, IScoreEntity
        {
            if (noteLayoutDictionary.TryGetValue(element.Guid, out var value))
            {
                return value.Copy();
            }

            return documentStyle.NoteLayoutFactory(element);
        }
        public ScoreMeasureLayout ScoreMeasureLayout<TElement>(TElement element) where TElement : IScoreMeasure, IScoreEntity
        {
            if (scoreMeasureLayoutDictionary.TryGetValue(element.Guid, out var value))
            {
                return value.Copy();
            }

            return documentStyle.ScoreMeasureLayoutFactory(element);
        }
        public StaffGroupLayout StaffGroupLayout<TElement>(TElement element) where TElement : IStaffGroup, IScoreEntity
        {
            if (staffGroupLayoutDictionary.TryGetValue(element.Guid, out var layout))
            {
                return layout.Copy();
            }

            return documentStyle.StaffGroupLayoutFactory(element);
        }
        public StaffLayout StaffLayout<TElement>(TElement element) where TElement : IStaff, IScoreEntity
        {
            if (staffLayoutDictionary.TryGetValue(element.Guid, out var layout))
            {
                return layout.Copy();
            }

            return documentStyle.StaffLayoutFactory(element);
        }
        public StaffSystemLayout StaffSystemLayout<TElement>(TElement element) where TElement : IStaffSystem, IScoreEntity
        {
            if (staffSystemLayoutDictionary.TryGetValue(element.Guid, out var layout))
            {
                return layout.Copy();
            }

            return documentStyle.StaffSystemLayoutFactory(element);
        }




        public void Restore(IChordEditor element)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RemoveLayoutCommand<ChordLayout>(chordLayoutDictionary, element.Guid);
            transaction.Enqueue(command);
        }
        public void Restore(IInstrumentMeasureEditor element)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RemoveLayoutCommand<InstrumentMeasureLayout>(instrumentMeasureLayoutDictionary, element.Guid);
            transaction.Enqueue(command);
        }
        public void Restore(IInstrumentRibbonEditor element)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RemoveLayoutCommand<InstrumentRibbonLayout>(instrumentRibbonLayoutDictionary, element.Guid);
            transaction.Enqueue(command);
        }
        public void Restore(IMeasureBlockEditor element)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RemoveLayoutCommand<MeasureBlockLayout>(measureBlockLayoutDictionary, element.Guid);
            transaction.Enqueue(command);
        }
        public void Restore(INoteEditor element)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RemoveLayoutCommand<NoteLayout>(noteLayoutDictionary, element.Guid);
            transaction.Enqueue(command);
        }
        public void Restore(IScoreMeasureEditor element)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RemoveLayoutCommand<ScoreMeasureLayout>(scoreMeasureLayoutDictionary, element.Guid);
            transaction.Enqueue(command);
        }
        public void Restore(IStaffEditor element)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RemoveLayoutCommand<StaffLayout>(staffLayoutDictionary, element.Guid);
            transaction.Enqueue(command);
        }
        public void Restore(IStaffGroupEditor element)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RemoveLayoutCommand<StaffGroupLayout>(staffGroupLayoutDictionary, element.Guid);
            transaction.Enqueue(command);
        }
        public void Restore(IStaffSystemEditor element)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RemoveLayoutCommand<StaffSystemLayout>(staffSystemLayoutDictionary, element.Guid);
            transaction.Enqueue(command);
        }
        public void Restore()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            ScoreDocumentLayout? oldLayout = null;
            var command = new SimpleCommand(
                () =>
                {
                    oldLayout = this.documentLayout;
                    if (oldLayout is not null)
                    {
                        this.documentLayout = null;
                    }
                },
                () =>
                {
                    this.documentLayout = oldLayout;
                });
            transaction.Enqueue(command);
        }
        public void Clean()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            transaction.Enqueue(new WipeDictionaryCommand<ChordLayout>(chordLayoutDictionary));
            transaction.Enqueue(new WipeDictionaryCommand<InstrumentMeasureLayout>(instrumentMeasureLayoutDictionary));
            transaction.Enqueue(new WipeDictionaryCommand<InstrumentRibbonLayout>(instrumentRibbonLayoutDictionary));
            transaction.Enqueue(new WipeDictionaryCommand<MeasureBlockLayout>(measureBlockLayoutDictionary));
            transaction.Enqueue(new WipeDictionaryCommand<NoteLayout>(noteLayoutDictionary));
            transaction.Enqueue(new WipeDictionaryCommand<ScoreMeasureLayout>(scoreMeasureLayoutDictionary));
            transaction.Enqueue(new WipeDictionaryCommand<StaffLayout>(staffLayoutDictionary));
            transaction.Enqueue(new WipeDictionaryCommand<StaffGroupLayout>(staffGroupLayoutDictionary));
            transaction.Enqueue(new WipeDictionaryCommand<StaffSystemLayout>(staffSystemLayoutDictionary));
        }
    }
}
