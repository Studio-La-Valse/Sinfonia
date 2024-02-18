using Sinfonia.Implementations.ScoreDocument.Layout.Elements;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    internal class ScoreLayoutDictionary
    {
        public IScoreDocumentLayout ScoreDocumentLayout { get; set; }


        private readonly Dictionary<Guid, INoteLayout> measureElementLayoutDictionary = [];
        private readonly Dictionary<Guid, IChordLayout> chordLayoutDictionary = [];
        private readonly Dictionary<Guid, IMeasureBlockLayout> chordGroupReaderDictionary = [];

        private readonly Dictionary<Guid, IInstrumentMeasureLayout> instrumentMeasureLayoutDictionary = [];
        private readonly Dictionary<Guid, IScoreMeasureLayout> scoreMeasureLayoutDictionary = [];
        private readonly Dictionary<Guid, IInstrumentRibbonLayout> instrumentRibbonLayoutDictionary = [];

        private readonly Dictionary<Guid, IStaffLayout> staffLayoutDictionary = [];
        private readonly Dictionary<Guid, IStaffGroupLayout> staffGroupLayoutDictionary = [];
        private readonly Dictionary<Guid, IStaffSystemLayout> staffSystemLayoutDictionary = [];



        public ScoreLayoutDictionary(IScoreDocumentLayout scoreDocumentLayout)
        {
            ScoreDocumentLayout = scoreDocumentLayout;
        }


        public INoteLayout GetOrCreate(Note element)
        {
            if (measureElementLayoutDictionary.TryGetValue(element.Guid, out var value))
            {
                return value;
            }

            var layout = new MeasureElementLayout();
            measureElementLayoutDictionary.Add(element.Guid, layout);

            return layout;
        }


        public IChordLayout GetOrCreate(Chord element)
        {
            if (chordLayoutDictionary.TryGetValue(element.Guid, out var value))
            {
                return value;
            }

            var layout = new ChordLayout();
            chordLayoutDictionary.Add(element.Guid, layout);

            return layout;
        }


        public IMeasureBlockLayout GetOrCreate(MeasureBlock reader)
        {
            if (chordGroupReaderDictionary.TryGetValue(reader.Guid, out var value))
            {
                return value;
            }

            var layout = new MeasureBlockLayout();
            chordGroupReaderDictionary[reader.Guid] = layout;

            return layout;
        }



        public IInstrumentMeasureLayout GetOrCreate(InstrumentMeasure element)
        {
            if (instrumentMeasureLayoutDictionary.TryGetValue(element.Guid, out var value))
            {
                return value;
            }

            var layout = new InstrumentMeasureLayout();
            instrumentMeasureLayoutDictionary.Add(element.Guid, layout);

            return layout;
        }



        public IScoreMeasureLayout GetOrCreate(ScoreMeasure element)
        {
            if (scoreMeasureLayoutDictionary.TryGetValue(element.Guid, out var value))
            {
                return value;
            }

            var layout = new ScoreMeasureLayout();
            scoreMeasureLayoutDictionary.Add(element.Guid, layout);

            return layout;
        }



        public IInstrumentRibbonLayout GetOrCreate(InstrumentRibbon element)
        {
            if (instrumentRibbonLayoutDictionary.TryGetValue(element.Guid, out var value))
            {
                return value;
            }

            var instrument = element.Instrument;
            var layout = new InstrumentRibbonLayout(instrument);
            instrumentRibbonLayoutDictionary.Add(element.Guid, layout);

            return layout;
        }



        public IStaffLayout GetOrCreate(Staff element)
        {
            if (staffLayoutDictionary.TryGetValue(element.Guid, out var layout))
            {
                return layout;
            }

            layout = new StaffLayout();
            staffLayoutDictionary.Add(element.Guid, layout);

            return layout;
        }



        public IStaffGroupLayout GetOrCreate(StaffGroup element)
        {
            if (staffGroupLayoutDictionary.TryGetValue(element.Guid, out var layout))
            {
                return layout;
            }

            layout = new StaffGroupLayout(element.Instrument);
            staffGroupLayoutDictionary.Add(element.Guid, layout);

            return layout;
        }



        public IStaffSystemLayout GetOrCreate(StaffSystem element)
        {
            if (staffSystemLayoutDictionary.TryGetValue(element.Guid, out var layout))
            {
                return layout;
            }

            layout = new StaffSystemLayout();
            staffSystemLayoutDictionary.Add(element.Guid, layout);

            return layout;
        }
    }
}
