using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class ScoreDocumentCore : ScoreElement, IMementoElement<ScoreDocumentMemento>
    {
        private readonly ScoreContentTable contentTable;
        private readonly PageGenerator pageGenerator;
        private readonly ScoreDocumentStyleTemplate styleTemplate;
        private readonly IKeyGenerator<int> keyGenerator;


        public int NumberOfMeasures =>
            contentTable.Width;
        public int NumberOfInstruments =>
            contentTable.Height;


        public ScoreDocumentLayout Layout { get; }


        internal ScoreDocumentCore(ScoreContentTable contentTable, PageGenerator pageGenerator, ScoreDocumentStyleTemplate styleTemplate, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.contentTable = contentTable;
            this.pageGenerator = pageGenerator;
            this.styleTemplate = styleTemplate;
            this.keyGenerator = keyGenerator;

            Layout = new ScoreDocumentLayout(styleTemplate);
        }




        public void AddInstrumentRibbon(Instrument instrument)
        {
            InstrumentRibbon instrumentRibbon = new(this, instrument, styleTemplate, keyGenerator, Guid.NewGuid());
            contentTable.AddInstrumentRibbon(instrumentRibbon);
        }
        public void RemoveInstrumentRibbon(int indexInScore)
        {
            contentTable.RemoveInstrumentRibbon(indexInScore);
        }

        public int IndexOf(ScoreMeasure scoreMeasure)
        {
            return contentTable.IndexOf(scoreMeasure);
        }
        public int IndexOf(InstrumentRibbon instrumentRibbon)
        {
            return contentTable.IndexOf(instrumentRibbon);
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

            ScoreMeasure scoreMeasure = new(this, timeSignature, styleTemplate, keyGenerator, guid);
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
        public IEnumerable<InstrumentMeasure> EnumerateScoreMeasuresCore(ScoreMeasure scoreMeasure)
        {
            return contentTable.GetInstrumentMeasuresInScoreMeasure(scoreMeasure.IndexInScore);
        }
        public IEnumerable<InstrumentMeasure> EnumerateScoreMeasuresCore(InstrumentRibbon instrumentRibbon)
        {
            return contentTable.GetInstrumentMeasuresInInstrumentRibbon(instrumentRibbon.IndexInScore);
        }
        public InstrumentMeasure GetMeasureCore(int scoreMeasureIndex, int ribbonIndex)
        {
            return contentTable.GetInstrumentMeasure(scoreMeasureIndex, ribbonIndex);
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
                Guid = Guid,
                Layout = Layout.GetMemento(),
                InstrumentRibbons = EnumerateRibbonsCore().Select(e => e.GetMemento()).ToList(),
                ScoreMeasures = EnumerateMeasuresCore().Select(e => e.GetMemento()).ToList()
            };
        }
        public void ApplyMemento(ScoreDocumentMemento memento)
        {
            Clear();

            foreach (var instrumentMemento in memento.InstrumentRibbons)
            {
                var instrumentRibbon = new InstrumentRibbon(this, instrumentMemento.Instrument, styleTemplate, keyGenerator, instrumentMemento.Guid);
                contentTable.AddInstrumentRibbon(instrumentRibbon);
            }

            foreach (var scoreMeasureMemento in memento.ScoreMeasures)
            {
                var scoreMeasure = CreateScoreMeasureCore(scoreMeasureMemento.Guid, scoreMeasureMemento.TimeSignature);
                contentTable.AddScoreMeasure(scoreMeasure);

                scoreMeasure.ApplyMemento(scoreMeasureMemento);
            }

            Layout.ApplyMemento(memento.Layout);
        }

        public IEnumerable<Page> GeneratePages()
        {
            return pageGenerator.Generate(this);
        }
    }
}

