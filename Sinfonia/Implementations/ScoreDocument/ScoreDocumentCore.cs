namespace Sinfonia.Implementations.ScoreDocument
{
    internal class ScoreDocumentCore : ScoreElement, IMementoElement<ScoreDocumentMemento>
    {
        internal readonly ScoreContentTable contentTable;
        private readonly PageGenerator pageGenerator;
        private readonly IKeyGenerator<int> keyGenerator;


        public int NumberOfMeasures =>
            contentTable.Width;
        public int NumberOfInstruments =>
            contentTable.Height;




        internal ScoreDocumentCore(ScoreContentTable contentTable, PageGenerator pageGenerator, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.contentTable = contentTable;
            this.pageGenerator = pageGenerator;
            this.keyGenerator = keyGenerator;
        }




        public void AddInstrumentRibbon(Instrument instrument)
        {
            InstrumentRibbon instrumentRibbon = new(this, instrument, keyGenerator, Guid.NewGuid());
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

            ScoreMeasure? previousElement = contentTable.ColumnHeaders.LastOrDefault();
            timeSignature ??= previousElement is not null ?
                    previousElement.TimeSignature :
                    new TimeSignature(4, 4);

            KeySignature keySignature = previousElement is not null ?
                    previousElement.KeySignature :
                    new KeySignature(Step.C, MajorOrMinor.Major);

            ScoreMeasure scoreMeasure = new(this, timeSignature, keySignature, keyGenerator, guid);
            return scoreMeasure;
        }
        public void AppendScoreMeasure(TimeSignature? timeSignature = null)
        {
            ScoreMeasure scoreMeasure = CreateScoreMeasureCore(Guid.NewGuid(), timeSignature);
            contentTable.AddScoreMeasure(scoreMeasure);
        }
        public void InsertScoreMeasure(int index, TimeSignature? timeSignature = null)
        {
            ScoreMeasure scoreMeasure = CreateScoreMeasureCore(Guid.NewGuid(), timeSignature);
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

            foreach (InstrumentRibbonMemento instrumentMemento in memento.InstrumentRibbons)
            {
                InstrumentRibbon instrumentRibbon = new(this, instrumentMemento.Instrument, keyGenerator, instrumentMemento.Guid);
                contentTable.AddInstrumentRibbon(instrumentRibbon);

                instrumentRibbon.ApplyMemento(instrumentMemento);
            }

            foreach (ScoreMeasureMemento scoreMeasureMemento in memento.ScoreMeasures)
            {
                ScoreMeasure scoreMeasure = CreateScoreMeasureCore(scoreMeasureMemento.Guid, scoreMeasureMemento.TimeSignature);
                contentTable.AddScoreMeasure(scoreMeasure);

                scoreMeasure.ApplyMemento(scoreMeasureMemento);
            }
        }

        public IEnumerable<Page> GeneratePages()
        {
            return pageGenerator.Generate(this);
        }
    }
}

