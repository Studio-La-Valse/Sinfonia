namespace Sinfonia.ViewModels.Application.Document.Inspector
{
    public class NotePropertiesViewModel : ScoreElementPropertiesViewModel<INoteReader, INoteEditor, NoteLayout>
    {
        internal NotePropertiesViewModel(IEnumerable<INoteReader> notes, IScoreBuilder scoreBuilder, IScoreDocumentLayout scoreLayoutDictionary) : base(scoreBuilder, scoreLayoutDictionary, notes)
        {
            Properties.Add(Create(l => l.StaffIndex, (l, v) => l.StaffIndex = v, "Staff Index"));
            Properties.Add(Create(l => l.XOffset, (l, v) => l.XOffset = v, "X Offset"));
            Properties.Add(Create(l => l.ForceAccidental, (l, v) => l.ForceAccidental = v, "Accidental"));
            Properties.Add(Create(l => l.Scale, (l, v) => l.Scale = v, "Scale"));
        }

        public override string Header => "Note Properties";

        public override NoteLayout GetLayout(IScoreDocumentLayout scoreLayoutProvider, INoteReader entity)
        {
            return scoreLayoutProvider.NoteLayout(entity);
        }
    }

    public class ChordPropertiesViewModel : ScoreElementPropertiesViewModel<IChordReader, IChordEditor, ChordLayout>
    {
        internal ChordPropertiesViewModel(IEnumerable<IChordReader> notes, IScoreBuilder scoreBuilder, IScoreDocumentLayout scoreLayoutDictionary) : base(scoreBuilder, scoreLayoutDictionary, notes)
        {
            Properties.Add(Create(l => l.XOffset, (l, v) => l.XOffset = v, "X Offset"));
        }

        public override string Header => "Chord Properties";

        public override ChordLayout GetLayout(IScoreDocumentLayout scoreLayoutProvider, IChordReader entity)
        {
            return scoreLayoutProvider.ChordLayout(entity);
        }
    }

    public class MeasureBlockPropertiesViewModel : ScoreElementPropertiesViewModel<IMeasureBlockReader, IMeasureBlockEditor, MeasureBlockLayout>
    {
        internal MeasureBlockPropertiesViewModel(IEnumerable<IMeasureBlockReader> notes, IScoreBuilder scoreBuilder, IScoreDocumentLayout scoreLayoutDictionary) : base(scoreBuilder, scoreLayoutDictionary, notes)
        {
            Properties.Add(Create(l => l.StemLength, (l, v) => l.StemLength = v, "Stem Length"));
            Properties.Add(Create(l => l.BeamAngle, (l, v) => l.BeamAngle = v, "Beam Angle"));
        }

        public override string Header => "Measure Block Properties";

        public override MeasureBlockLayout GetLayout(IScoreDocumentLayout scoreLayoutProvider, IMeasureBlockReader entity)
        {
            return scoreLayoutProvider.MeasureBlockLayout(entity);
        }
    }

    public class InstrumentMeasurePropertiesViewModel : ScoreElementPropertiesViewModel<IInstrumentMeasureReader, IInstrumentMeasureEditor, InstrumentMeasureLayout>
    {
        internal InstrumentMeasurePropertiesViewModel(IEnumerable<IInstrumentMeasureReader> notes, IScoreBuilder scoreBuilder, IScoreDocumentLayout scoreLayoutDictionary) : base(scoreBuilder, scoreLayoutDictionary, notes)
        {

        }

        public override string Header => "Instrument Measure Properties";

        public override InstrumentMeasureLayout GetLayout(IScoreDocumentLayout scoreLayoutProvider, IInstrumentMeasureReader entity)
        {
            return scoreLayoutProvider.InstrumentMeasureLayout(entity);
        }
    }

    public class ScoreMeasurePropertiesViewModel : ScoreElementPropertiesViewModel<IScoreMeasureReader, IScoreMeasureEditor, ScoreMeasureLayout>
    {
        internal ScoreMeasurePropertiesViewModel(IEnumerable<IScoreMeasureReader> notes, IScoreBuilder scoreBuilder, IScoreDocumentLayout scoreLayoutDictionary) : base(scoreBuilder, scoreLayoutDictionary, notes)
        {
            Properties.Add(Create(l => l.KeySignature, (l, v) => l.KeySignature = v, "Key Signature"));
            Properties.Add(Create(l => l.PaddingLeft, (l, v) => l.PaddingLeft = v, "Padding Left"));
            Properties.Add(Create(l => l.PaddingRight, (l, v) => l.PaddingRight = v, "Padding Right"));
            Properties.Add(Create(l => l.Width, (l, v) => l.Width = v, "Width"));
        }

        public override string Header => "Score Measure Properties";

        public override ScoreMeasureLayout GetLayout(IScoreDocumentLayout scoreLayoutProvider, IScoreMeasureReader entity)
        {
            return scoreLayoutProvider.ScoreMeasureLayout(entity);
        }
    }

    public class InstrumentRibbonPropertiesViewModel : ScoreElementPropertiesViewModel<IInstrumentRibbonReader, IInstrumentRibbonEditor, InstrumentRibbonLayout>
    {
        internal InstrumentRibbonPropertiesViewModel(IEnumerable<IInstrumentRibbonReader> notes, IScoreBuilder scoreBuilder, IScoreDocumentLayout scoreLayoutDictionary) : base(scoreBuilder, scoreLayoutDictionary, notes)
        {
            Properties.Add(Create(l => l.AbbreviatedName, (l, v) => l.AbbreviatedName = v, "Nick Name"));
            Properties.Add(Create(l => l.DisplayName, (l, v) => l.DisplayName = v, "Instrument Name"));
            Properties.Add(Create(l => l.NumberOfStaves, (l, v) => l.NumberOfStaves = v, "Number of Staves"));
            Properties.Add(Create(l => l.Collapsed, (l, v) => l.Collapsed = v, "Collapsed"));
        }

        public override string Header => "Instrument Ribbon Properties";

        public override InstrumentRibbonLayout GetLayout(IScoreDocumentLayout scoreLayoutProvider, IInstrumentRibbonReader entity)
        {
            return scoreLayoutProvider.InstrumentRibbonLayout(entity);
        }
    }

    public class StaffPropertiesViewModel : ScoreElementPropertiesViewModel<IStaffReader, IStaffEditor, StaffLayout>
    {
        internal StaffPropertiesViewModel(IEnumerable<IStaffReader> notes, IScoreBuilder scoreBuilder, IScoreDocumentLayout scoreLayoutDictionary) : base(scoreBuilder, scoreLayoutDictionary, notes)
        {
            Properties.Add(Create(l => l.DistanceToNext, (l, v) => l.DistanceToNext = v, "Margin Below"));
        }

        public override string Header => "Staff Properties";

        public override StaffLayout GetLayout(IScoreDocumentLayout scoreLayoutProvider, IStaffReader entity)
        {
            return scoreLayoutProvider.StaffLayout(entity);
        }
    }

    public class StaffGroupPropertiesViewModel : ScoreElementPropertiesViewModel<IStaffGroupReader, IStaffGroupEditor, StaffGroupLayout>
    {
        internal StaffGroupPropertiesViewModel(IEnumerable<IStaffGroupReader> notes, IScoreBuilder scoreBuilder, IScoreDocumentLayout scoreLayoutDictionary) : base(scoreBuilder, scoreLayoutDictionary, notes)
        {
            Properties.Add(Create(l => l.Collapsed, (l, v) => l.Collapsed = v, "Collapsed"));
            Properties.Add(Create(l => l.NumberOfStaves, (l, v) => l.NumberOfStaves = v, "Number of Staves"));
            Properties.Add(Create(l => l.DistanceToNext, (l, v) => l.DistanceToNext = v, "Margin Below"));
            Properties.Add(Create(l => l.LineSpacing, (l, v) => l.LineSpacing = v, "Line Spacing"));
        }

        public override string Header => "Staff Group Properties";

        public override StaffGroupLayout GetLayout(IScoreDocumentLayout scoreLayoutProvider, IStaffGroupReader entity)
        {
            return scoreLayoutProvider.StaffGroupLayout(entity);
        }
    }

    public class StaffSystemPropertiesViewModel : ScoreElementPropertiesViewModel<IStaffSystemReader, IStaffSystemEditor, StaffSystemLayout>
    {
        internal StaffSystemPropertiesViewModel(IEnumerable<IStaffSystemReader> notes, IScoreBuilder scoreBuilder, IScoreDocumentLayout scoreLayoutDictionary) : base(scoreBuilder, scoreLayoutDictionary, notes)
        {
            Properties.Add(Create(l => l.PaddingBottom, (l, v) => l.PaddingBottom = v, "Margin Below"));
        }

        public override string Header => "Staff System Properties";

        public override StaffSystemLayout GetLayout(IScoreDocumentLayout scoreLayoutProvider, IStaffSystemReader entity)
        {
            return scoreLayoutProvider.StaffSystemLayout(entity);
        }
    }

    public class PagePropertiesViewModel : ScoreElementPropertiesViewModel<IPageReader, IPageEditor, PageLayout>
    {
        internal PagePropertiesViewModel(IEnumerable<IPageReader> notes, IScoreBuilder scoreBuilder, IScoreDocumentLayout scoreLayoutDictionary) : base(scoreBuilder, scoreLayoutDictionary, notes)
        {
            Properties.Add(Create(l => l.PageWidth, (l, v) => l.PageWidth = v, "Page Width"));
            Properties.Add(Create(l => l.PageHeight, (l, v) => l.PageHeight = v, "Page Height"));
            Properties.Add(Create(l => l.MarginLeft, (l, v) => l.MarginLeft = v, "Margin Left"));
            Properties.Add(Create(l => l.MarginTop, (l, v) => l.MarginTop = v, "Margin Top"));
            Properties.Add(Create(l => l.MarginRight, (l, v) => l.MarginRight = v, "Margin Right"));
            Properties.Add(Create(l => l.MarginBottom, (l, v) => l.MarginBottom = v, "Margin Below"));
        }

        public override string Header => "Page Properties";

        public override PageLayout GetLayout(IScoreDocumentLayout scoreLayoutProvider, IPageReader entity)
        {
            return scoreLayoutProvider.PageLayout(entity);
        }
    }

    public class ScoreDocumentPropertiesViewModel : ScoreElementPropertiesViewModel<IScoreDocumentReader, IScoreDocumentEditor, ScoreDocumentLayout>
    {
        internal ScoreDocumentPropertiesViewModel(IEnumerable<IScoreDocumentReader> notes, IScoreBuilder scoreBuilder, IScoreDocumentLayout scoreLayoutDictionary) : base(scoreBuilder, scoreLayoutDictionary, notes)
        {

        }

        public override string Header => "Score Document Properties";

        public override ScoreDocumentLayout GetLayout(IScoreDocumentLayout scoreLayoutProvider, IScoreDocumentReader entity)
        {
            return scoreLayoutProvider.DocumentLayout(entity);
        }
    }
}
