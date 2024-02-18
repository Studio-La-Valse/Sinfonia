using Sinfonia.Implementations.ScoreDocument.Layout;
using Sinfonia.Implementations.ScoreDocument.Layout.Elements;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class ScoreMeasure : ScoreElement, IMementoElement<ScoreMeasureMemento>, ILayoutElement<IScoreMeasureLayout>
    {
        private readonly ScoreDocumentCore score;
        private IScoreMeasureLayout layout;

        public TimeSignature TimeSignature { get; }
        public StaffSystem StaffSystemOrigin { get; }
        public Guid Guid { get; }


        public int IndexInScore =>
            score.contentTable.IndexOf(this);
        public bool IsLastInScore =>
            IndexInScore == score.NumberOfMeasures - 1;

        public KeySignature KeySignature { get => ReadLayout().KeySignature; set => ReadLayout().KeySignature = value; }
        public double PaddingLeft { get => ReadLayout().PaddingLeft; set => ReadLayout().PaddingLeft = value; }
        public double PaddingRight { get => ReadLayout().PaddingRight; set => ReadLayout().PaddingRight = value; }
        public double Width { get => ReadLayout().Width; set => ReadLayout().Width = value; }
        public bool IsNewSystem { get => ReadLayout().IsNewSystem; set => ReadLayout().IsNewSystem = value; }


        internal ScoreMeasure(ScoreDocumentCore score, TimeSignature timeSignature, IScoreMeasureLayout layout, IKeyGenerator<int> keyGenerator, Guid guid, Guid guidStaffSystem) : base(keyGenerator)
        {
            this.score = score;
            this.layout = layout;

            StaffSystemOrigin = new StaffSystem(keyGenerator, new StaffSystemLayout(), this, guidStaffSystem);
            foreach (var instrumentRibbon in score.EnumerateRibbonsCore())
            {
                StaffSystemOrigin.Register(instrumentRibbon);
            }

            TimeSignature = timeSignature;
            Guid = guid;
        }



        public IEnumerable<InstrumentMeasure> EnumerateMeasuresCore()
        {
            var measures = score.contentTable.GetCellsColumn(IndexInScore);
            return measures;
        }
        public InstrumentMeasure GetMeasureCore(int ribbonIndex)
        {
            return score.contentTable.GetCell(IndexInScore, ribbonIndex);
        }
        public bool TryReadPrevious([NotNullWhen(true)] out ScoreMeasure? previous)
        {
            previous = null;
            if (IndexInScore == 0)
            {
                return false;
            }

            try
            {
                previous = score.contentTable.ColumnAt(IndexInScore - 1);
            }
            catch
            {

            }

            return previous is not null;
        }
        public bool TryReadNext([NotNullWhen(true)] out ScoreMeasure? next)
        {
            next = null;
            if (IndexInScore + 1 >= score.NumberOfMeasures)
            {
                return false;
            }

            try
            {
                next = score.contentTable.ColumnAt(IndexInScore + 1);
            }
            catch { }

            return next is not null;
        }
        public StaffSystem GetStaffSystemOrigin()
        {
            return StaffSystemOrigin;
        }



        public IScoreMeasureLayout ReadLayout()
        {
            return layout;
        }
        public void ApplyLayout(IScoreMeasureLayout memento)
        {
            layout = memento;
        }


        public ScoreMeasureMemento GetMemento()
        {
            return new ScoreMeasureMemento
            {
                Measures = EnumerateMeasuresCore().Select(e => e.GetMemento()).ToList(),
                Layout = ReadLayout().Copy(),
                TimeSignature = TimeSignature,
                StaffSystem = StaffSystemOrigin.GetMemento(),
                Guid = Guid
            };
        }
        public void ApplyMemento(ScoreMeasureMemento memento)
        {
            ApplyLayout(memento.Layout);
            StaffSystemOrigin.ApplyMemento(memento.StaffSystem);

            foreach (var measureMemento in memento.Measures)
            {
                var measure = GetMeasureCore(measureMemento.RibbonIndex);
                measure.ApplyMemento(measureMemento);
            }
        }


        public override IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            yield return StaffSystemOrigin;
        }
    }
}
