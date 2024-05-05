using Sinfonia.Implementations.ScoreDocument.Memento.Layout;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    internal class InstrumentMeasureLayout : IInstrumentMeasureLayout, ILayout<InstrumentMeasureLayoutMemento>
    {
        private readonly HashSet<ClefChange> changeList = [];
        private readonly Dictionary<int, double> paddingBottomForStaves = [];
        private readonly ValueTemplateProperty<int> numberOfStaves;
        private readonly InstrumentMeasure instrumentMeasure;

        public int? NumberOfStaves { get; set; }
        public double? PaddingBottom { get; set; }
        public bool Collapsed { get; set; }


        public KeySignature KeySignature => 
            instrumentMeasure.ScoreMeasure.Layout.KeySignature;
        public IEnumerable<ClefChange> ClefChanges => 
            changeList;


        public InstrumentMeasureLayout(InstrumentMeasure instrumentMeasure)
        {
            this.numberOfStaves = new ValueTemplateProperty<int>(() => instrumentMeasure.Instrument.NumberOfStaves);
            this.instrumentMeasure = instrumentMeasure;
        }

        public void AddClefChange(ClefChange clefChange)
        {
            _ = changeList.Add(clefChange);
        }

        public void RemoveClefChange(ClefChange clefChange)
        {
            _ = changeList.Remove(clefChange);
        }

        public void RequestPaddingBottom(int staffIndex, double paddingBottom)
        {
            paddingBottomForStaves[staffIndex] = paddingBottom;
        }

        public double? GetPaddingBottom(int staffIndex)
        {
            if(paddingBottomForStaves.TryGetValue(staffIndex, out var paddingBottom))
            { 
                return paddingBottom; 
            }

            return null;
        }

        public InstrumentMeasureLayoutMemento GetMemento()
        {
            var clefChangeDictionary = new HashSet<ClefChange>();
            foreach (var clefChange in changeList)
            {
                clefChangeDictionary.Add(clefChange);
            }

            var paddingBottomStavesDictionary = new Dictionary<int, double>();
            foreach (var kv in paddingBottomForStaves)
            {
                paddingBottomStavesDictionary.Add(kv.Key, kv.Value);
            }

            return new InstrumentMeasureLayoutMemento()
            {
                ClefChanges = clefChangeDictionary,
                StaffPaddingBottom = paddingBottomStavesDictionary,
                NumberofStaves = numberOfStaves.Field,
                PaddingBottom = PaddingBottom,
                Collapsed = Collapsed
            };
        }

        public void ApplyMemento(InstrumentMeasureLayoutMemento? memento)
        {
            Restore();

            if (memento is null)
            {
                return;
            }

            foreach (var clefChange in memento.ClefChanges)
            {
                changeList.Add(clefChange);
            }

            foreach (var kv in memento.StaffPaddingBottom)
            {
                paddingBottomForStaves.Add(kv.Key, kv.Value);
            }

            numberOfStaves.Field = memento.NumberofStaves;
            PaddingBottom = memento.PaddingBottom;
            Collapsed = memento.Collapsed;
        }

        public void Restore()
        {
            changeList.Clear();
            paddingBottomForStaves.Clear();
            numberOfStaves.Reset();
            PaddingBottom = null;
            Collapsed = false;
        }
    }
}