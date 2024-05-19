using Sinfonia.Implementations.ScoreDocument.Converters;
using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;
using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.Primitives;
using ColorARGB = StudioLaValse.ScoreDocument.Layout.Templates.ColorARGB;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class ScoreDocumentCore : ScoreElement, IMementoElement<ScoreDocumentModel>
    {
        private readonly ScoreContentTable contentTable;
        private readonly PageGenerator pageGenerator;
        private readonly ScoreDocumentStyleTemplate styleTemplate;
        private readonly IKeyGenerator<int> keyGenerator;


        public int NumberOfMeasures =>
            contentTable.Width;
        public int NumberOfInstruments =>
            contentTable.Height;



        public PrimaryScoreDocumentLayout PrimaryLayout { get; }
        public SecondaryScoreDocumentLayout SecondaryLayout { get; }



        internal ScoreDocumentCore(ScoreContentTable contentTable,
                                   PageGenerator pageGenerator,
                                   ScoreDocumentStyleTemplate styleTemplate,
                                   PrimaryScoreDocumentLayout primaryLayout,
                                   SecondaryScoreDocumentLayout secondaryLayout,
                                   IKeyGenerator<int> keyGenerator,
                                   Guid guid) : base(keyGenerator, guid)
        {
            this.contentTable = contentTable;
            this.pageGenerator = pageGenerator;
            this.styleTemplate = styleTemplate;
            this.keyGenerator = keyGenerator;

            PrimaryLayout = primaryLayout;
            SecondaryLayout = secondaryLayout;
        }




        public void AddInstrumentRibbon(Instrument instrument)
        {
            var layoutId = Guid.NewGuid();
            var ribbonId = Guid.NewGuid();
            var instrumentRibbon = CreateInstrumentRibbonCore(instrument, ribbonId, layoutId);
            contentTable.AddInstrumentRibbon(instrumentRibbon);
        }
        public InstrumentRibbon CreateInstrumentRibbonCore(Instrument instrument, Guid ribbonId, Guid layoutId)
        {
            var primaryLayout = new InstrumentRibbonLayout(instrument);
            var secondaryLayout = new SecondaryInstrumentRibbonLayout(primaryLayout, layoutId);
            var instrumentRibbon = new InstrumentRibbon(this, instrument, primaryLayout, secondaryLayout, keyGenerator, ribbonId);
            return instrumentRibbon;
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


        public ScoreMeasure CreateScoreMeasureCore(Guid guid, Guid layoutGuid, TimeSignature? timeSignature = null)
        {
            if (!contentTable.RowHeaders.Any())
            {
                throw new Exception("Please construct an instrument ribbon first");
            }

            var previousElement = contentTable.ColumnHeaders.LastOrDefault();
            timeSignature ??= previousElement is not null ?
                    previousElement.TimeSignature :
                    new TimeSignature(4, 4);

            var layout = new PrimaryScoreMeasureLayout(styleTemplate.ScoreMeasureStyleTemplate);
            var secondaryLayout = new SecondaryScoreMeasureLayout(layoutGuid, layout);
            ScoreMeasure scoreMeasure = new(this, timeSignature, styleTemplate, layout, secondaryLayout, keyGenerator, guid);
            return scoreMeasure;
        }
        public void AppendScoreMeasure(TimeSignature? timeSignature = null)
        {
            var scoreMeasure = CreateScoreMeasureCore(Guid.NewGuid(), Guid.NewGuid(), timeSignature);
            contentTable.AddScoreMeasure(scoreMeasure);
        }
        public void InsertScoreMeasure(int index, TimeSignature? timeSignature = null)
        {
            var scoreMeasure = CreateScoreMeasureCore(Guid.NewGuid(), Guid.NewGuid(), timeSignature);
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


        public ScoreDocumentModel GetMemento()
        {
            return new ScoreDocumentModel
            {
                Id = Guid,
                Layout = SecondaryLayout.GetMemento(),
                InstrumentRibbons = EnumerateRibbonsCore().Select(e => e.GetMemento()).ToList(),
                ScoreMeasures = EnumerateMeasuresCore().Select(e => e.GetMemento()).ToList(),
                FirstSystemIndent = PrimaryLayout._FirstSystemIndent.Field,
                ForegroundColor = PrimaryLayout._ForegroundColor.Field?.Convert(),
                PageColor = PrimaryLayout._PageColor.Field?.Convert(),
                HorizontalStaffLineThickness = PrimaryLayout._HorizontalStaffLineThickness.Field,
                InstrumentScales = PrimaryLayout._InstrumentScalesSource.DeepCopy(),
                Scale = PrimaryLayout._Scale.Field,
                StemLineThickness = PrimaryLayout._StemLineThickness.Field,
                VerticalStaffLineThickness = PrimaryLayout._VerticalStaffLineThickness.Field,
            };
        }
        public void ApplyMemento(ScoreDocumentModel memento)
        {
            Clear();

            PrimaryLayout.Restore();
            PrimaryLayout.ApplyMemento(memento);
            SecondaryLayout.Restore();
            SecondaryLayout.ApplyMemento(memento.Layout);

            foreach (var instrumentMemento in memento.InstrumentRibbons)
            {
                var instrumentRibbon = CreateInstrumentRibbonCore(instrumentMemento.Instrument.Convert(), instrumentMemento.Id, instrumentMemento.Layout?.Id ?? Guid.NewGuid());
                contentTable.AddInstrumentRibbon(instrumentRibbon);

                instrumentRibbon.ApplyMemento(instrumentMemento);
            }

            foreach (var scoreMeasureMemento in memento.ScoreMeasures)
            {
                var scoreMeasure = CreateScoreMeasureCore(scoreMeasureMemento.Id, scoreMeasureMemento.Layout?.Id ?? Guid.NewGuid(), scoreMeasureMemento.TimeSignature.Convert());
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

