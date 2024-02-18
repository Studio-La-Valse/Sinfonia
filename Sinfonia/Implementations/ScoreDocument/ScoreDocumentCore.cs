using Sinfonia.Implementations.ScoreDocument.Layout.Elements;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class ScoreDocumentCore : ScoreElement, IMementoElement<ScoreDocumentMemento>
    {
        internal readonly Table<InstrumentMeasure, ScoreMeasure, InstrumentRibbon> contentTable;
        private readonly IKeyGenerator<int> keyGenerator;


        public int NumberOfMeasures =>
            contentTable.Width;
        public int NumberOfInstruments =>
            contentTable.Height;


        public Guid Guid { get; }



        internal ScoreDocumentCore(Table<InstrumentMeasure, ScoreMeasure, InstrumentRibbon> contentTable, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator)
        {
            this.contentTable = contentTable;
            this.keyGenerator = keyGenerator;

            Guid = guid;
        }




        public void AddInstrumentRibbon(Instrument instrument)
        {
            var layout = new InstrumentRibbonLayout(instrument);
            var instrumentRibbon = new InstrumentRibbon(this, instrument, layout, keyGenerator, Guid.NewGuid());
            contentTable.AddRow(instrumentRibbon);

            foreach (var staffSystem in contentTable.ColumnHeaders.Select(m => m.StaffSystemOrigin))
            {
                staffSystem.Register(instrumentRibbon);
            }
        }
        public void RemoveInstrumentRibbon(int indexInScore)
        {
            var ribbon = contentTable.RowAt(indexInScore);
            foreach (var staffSystem in contentTable.ColumnHeaders.Select(m => m.StaffSystemOrigin))
            {
                staffSystem.Unregister(ribbon);
            }

            contentTable.RemoveRow(ribbon.IndexInScore);
        }


        public ScoreMeasure CreateScoreMeasureCore(Guid guidMeasure, Guid guidStaffSystem, TimeSignature? timeSignature = null)
        {
            if (!contentTable.RowHeaders.Any())
            {
                throw new Exception("Please construct an instrument ribbon first");
            }

            var previousElement = contentTable.ColumnHeaders.LastOrDefault();
            timeSignature ??= previousElement is not null ?
                    previousElement.TimeSignature :
                    new TimeSignature(4, 4);

            var keySignature = previousElement is not null ?
                    previousElement.ReadLayout().KeySignature :
                    new KeySignature(Step.C, MajorOrMinor.Major);

            var layout = new ScoreMeasureLayout(keySignature);

            var scoreMeasure = new ScoreMeasure(this, timeSignature, layout, keyGenerator, guidMeasure, guidStaffSystem);
            return scoreMeasure;
        }
        public void AppendScoreMeasure(TimeSignature? timeSignature = null)
        {
            var scoreMeasure = CreateScoreMeasureCore(Guid.NewGuid(), Guid.NewGuid(), timeSignature);
            contentTable.AddColumn(scoreMeasure);
        }
        public void InsertScoreMeasure(int index, TimeSignature? timeSignature = null)
        {
            var scoreMeasure = CreateScoreMeasureCore(Guid.NewGuid(), Guid.NewGuid(), timeSignature);
            contentTable.InsertColumn(scoreMeasure, index);
        }
        public void RemoveScoreMeasure(int indexInScore)
        {
            contentTable.RemoveColumn(indexInScore);
        }


        public void Clear()
        {
            while (contentTable.Height > 0)
            {
                contentTable.RemoveRow(0);
            }

            while (contentTable.Width > 0)
            {
                contentTable.RemoveColumn(0);
            }
        }


        public IEnumerable<ScoreMeasure> EnumerateMeasuresCore()
        {
            return contentTable.ColumnHeaders;
        }
        public IEnumerable<InstrumentRibbon> EnumerateRibbonsCore()
        {
            return contentTable.RowHeaders;
        }
        public IEnumerable<StaffSystem> EnumerateStaffSystemsCore()
        {
            if (!EnumerateMeasuresCore().Any())
            {
                yield break;
            }

            var firstMeasure = contentTable.ColumnHeaders.First();
            if (!firstMeasure.ReadLayout().IsNewSystem)
            {
                throw new Exception("The first measure of the score must have a new system in it's layout supplied before the score can enumerate the staff systems.");
            }

            foreach (var measure in contentTable.ColumnHeaders)
            {
                if (measure.ReadLayout().IsNewSystem)
                {
                    yield return measure.StaffSystemOrigin;
                }
            }
        }




        public ScoreMeasure GetScoreMeasureCore(int indexInScore)
        {
            return contentTable.ColumnAt(indexInScore);
        }
        public InstrumentRibbon GetInstrumentRibbonCore(int indexInScore)
        {
            return contentTable.RowAt(indexInScore);
        }




        public ScoreDocumentMemento GetMemento()
        {
            return new ScoreDocumentMemento
            {
                InstrumentRibbons = EnumerateRibbonsCore().Select(e => e.GetMemento()).ToList(),
                ScoreMeasures = EnumerateMeasuresCore().Select(e => e.GetMemento()).ToList()
            };
        }
        public void ApplyMemento(ScoreDocumentMemento memento)
        {
            Clear();

            foreach (var instrumentMemento in memento.InstrumentRibbons)
            {
                var layout = new InstrumentRibbonLayout(instrumentMemento.Instrument);
                var instrumentRibbon = new InstrumentRibbon(this, instrumentMemento.Instrument, layout, keyGenerator, instrumentMemento.Guid);
                contentTable.AddRow(instrumentRibbon);

                foreach (var staffSystem in contentTable.ColumnHeaders.Select(m => m.StaffSystemOrigin))
                {
                    staffSystem.Register(instrumentRibbon);
                }

                instrumentRibbon.ApplyMemento(instrumentMemento);
            }

            foreach (var scoreMeasureMemento in memento.ScoreMeasures)
            {
                var scoreMeasure = CreateScoreMeasureCore(scoreMeasureMemento.Guid, scoreMeasureMemento.StaffSystem.Guid, scoreMeasureMemento.TimeSignature);
                contentTable.AddColumn(scoreMeasure);

                scoreMeasure.ApplyMemento(scoreMeasureMemento);
            }
        }



        public override IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            foreach (var ribbon in EnumerateRibbonsCore())
            {
                yield return ribbon;
            }

            foreach (var measure in EnumerateMeasuresCore())
            {
                yield return measure;
            }
        }
    }
}

