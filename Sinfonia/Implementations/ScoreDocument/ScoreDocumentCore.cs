using Sinfonia.Implementations.ScoreDocument.Proxy.Reader;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class ScoreDocumentCore : ScoreElement, IMementoElement<ScoreDocumentMemento>
    {
        private readonly ScoreLayoutDictionary scoreLayoutDictionary;
        internal readonly ScoreContentTable contentTable;
        private readonly StaffSystemGenerator staffSystemGenerator;
        private readonly IKeyGenerator<int> keyGenerator;


        public int NumberOfMeasures =>
            contentTable.Width;
        public int NumberOfInstruments =>
            contentTable.Height;




        internal ScoreDocumentCore(ScoreLayoutDictionary scoreLayoutDictionary, ScoreContentTable contentTable, StaffSystemGenerator staffSystemGenerator, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.scoreLayoutDictionary = scoreLayoutDictionary;
            this.contentTable = contentTable;
            this.staffSystemGenerator = staffSystemGenerator;
            this.keyGenerator = keyGenerator;
        }




        public void AddInstrumentRibbon(Instrument instrument)
        {
            var instrumentRibbon = new InstrumentRibbon(this, instrument, keyGenerator, Guid.NewGuid());
            contentTable.AddInstrumentRibbon(instrumentRibbon);
        }
        public void RemoveInstrumentRibbon(int indexInScore)
        {
            contentTable.RemoveInstrumentRibbon(indexInScore);
        }


        public ScoreMeasure CreateScoreMeasureCore(Guid guid, TimeSignature? timeSignature = null)
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
                    previousElement.KeySignature :
                    new KeySignature(Step.C, MajorOrMinor.Major);

            var scoreMeasure = new ScoreMeasure(this, timeSignature, keySignature, keyGenerator, guid);
            return scoreMeasure;
        }
        public void AppendScoreMeasure(TimeSignature? timeSignature = null)
        {
            var scoreMeasure = CreateScoreMeasureCore(Guid.NewGuid(), timeSignature);
            contentTable.AddScoreMeasure(scoreMeasure);
        }
        public void InsertScoreMeasure(int index, TimeSignature? timeSignature = null)
        {
            var scoreMeasure = CreateScoreMeasureCore(Guid.NewGuid(), timeSignature);
            contentTable.InsertScoreMeasure(scoreMeasure, index);
        }
        public void RemoveScoreMeasure(int indexInScore)
        {
            contentTable.RemoveScoreMeasure(indexInScore);
        }


        public void Clear()
        {
            while (contentTable.Height > 0)
            {
                contentTable.RemoveInstrumentRibbon(0);
            }

            while (contentTable.Width > 0)
            {
                contentTable.RemoveScoreMeasure(0);
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



        public ScoreMeasure GetScoreMeasureCore(int indexInScore)
        {
            return contentTable.ScoreMeasureAt(indexInScore);
        }
        public InstrumentRibbon GetInstrumentRibbonCore(int indexInScore)
        {
            return contentTable.InstrumentRibbonAt(indexInScore);
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
                var instrumentRibbon = new InstrumentRibbon(this, instrumentMemento.Instrument, keyGenerator, instrumentMemento.Guid);
                contentTable.AddInstrumentRibbon(instrumentRibbon);

                instrumentRibbon.ApplyMemento(instrumentMemento);
            }

            foreach (var scoreMeasureMemento in memento.ScoreMeasures)
            {
                var scoreMeasure = CreateScoreMeasureCore(scoreMeasureMemento.Guid, scoreMeasureMemento.TimeSignature);
                contentTable.AddScoreMeasure(scoreMeasure);

                scoreMeasure.ApplyMemento(scoreMeasureMemento);
            }
        }


        public IEnumerable<StaffSystem> EnumerateStaffSystems()
        {
            return staffSystemGenerator.EnumerateStaffSystems(this, scoreLayoutDictionary.DocumentLayout(this.Proxy()));
        }
    }
}

