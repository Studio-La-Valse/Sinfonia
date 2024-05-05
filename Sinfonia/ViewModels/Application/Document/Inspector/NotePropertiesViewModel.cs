using StudioLaValse.ScoreDocument.Reader;

namespace Sinfonia.ViewModels.Application.Document.Inspector
{
    public class NotePropertiesViewModel : ScoreElementPropertiesViewModel<INoteReader, INoteEditor>
    {
        internal NotePropertiesViewModel(IEnumerable<INoteReader> notes, IScoreBuilder scoreBuilder) : base(scoreBuilder, notes)
        {
            Properties.Add(Create(l => l.ReadLayout().StaffIndex, (l, v) => l.SetStaffIndex(v), "Staff Index"));
            Properties.Add(Create(l => l.ReadLayout().XOffset, (l, v) => l.SetXOffset(v), "X Offset"));
            Properties.Add(Create(l => l.ReadLayout().ForceAccidental, (l, v) => l.SetForceAccidental(v), "Accidental"));
            Properties.Add(Create(l => l.ReadLayout().Scale, (l, v) => l.SetScale(v), "Scale"));
        }

        public override string Header => "Note Properties";
    }

    public class ChordPropertiesViewModel : ScoreElementPropertiesViewModel<IChordReader, IChordEditor>
    {
        internal ChordPropertiesViewModel(IEnumerable<IChordReader> notes, IScoreBuilder scoreBuilder) : base(scoreBuilder, notes)
        {
            Properties.Add(Create(l => l.ReadLayout().XOffset, (l, v) => l.SetXOffset(v), "X Offset"));
        }

        public override string Header => "Chord Properties";
    }

    public class MeasureBlockPropertiesViewModel : ScoreElementPropertiesViewModel<IMeasureBlockReader, IMeasureBlockEditor>
    {
        internal MeasureBlockPropertiesViewModel(IEnumerable<IMeasureBlockReader> notes, IScoreBuilder scoreBuilder) : base(scoreBuilder, notes)
        {
            Properties.Add(Create(l => l.ReadLayout().StemLength, (l, v) => l.SetStemLength(v), "Stem Length"));
            Properties.Add(Create(l => l.ReadLayout().BeamAngle, (l, v) => l.SetBeamAngle(v), "Beam Angle"));
        }

        public override string Header => "Measure Block Properties";
    }

    public class InstrumentMeasurePropertiesViewModel : ScoreElementPropertiesViewModel<IInstrumentMeasureReader, IInstrumentMeasureEditor>
    {
        internal InstrumentMeasurePropertiesViewModel(IEnumerable<IInstrumentMeasureReader> notes, IScoreBuilder scoreBuilder) : base(scoreBuilder, notes)
        {

        }

        public override string Header => "Instrument Measure Properties";
    }

    public class ScoreMeasurePropertiesViewModel : ScoreElementPropertiesViewModel<IScoreMeasureReader, IScoreMeasureEditor>
    {
        internal ScoreMeasurePropertiesViewModel(IEnumerable<IScoreMeasureReader> notes, IScoreBuilder scoreBuilder) : base(scoreBuilder, notes)
        {
            Properties.Add(Create(l => l.ReadLayout().KeySignature, (l, v) => l.SetKeySignature(v), "Key Signature"));
            Properties.Add(Create(l => l.ReadLayout().PaddingLeft, (l, v) => l.SetPaddingLeft(v), "Padding Left"));
            Properties.Add(Create(l => l.ReadLayout().PaddingRight, (l, v) => l.SetPaddingRight(v), "Padding Right"));
            Properties.Add(Create(l => l.ReadLayout().Width, (l, v) => l.SetWidth(v), "Width"));
        }

        public override string Header => "Score Measure Properties";
    }

    public class InstrumentRibbonPropertiesViewModel : ScoreElementPropertiesViewModel<IInstrumentRibbonReader, IInstrumentRibbonEditor>
    {
        internal InstrumentRibbonPropertiesViewModel(IEnumerable<IInstrumentRibbonReader> notes, IScoreBuilder scoreBuilder) : base(scoreBuilder, notes)
        {
            Properties.Add(Create(l => l.ReadLayout().AbbreviatedName, (l, v) => l.SetAbbreviatedName(v), "Nick Name"));
            Properties.Add(Create(l => l.ReadLayout().DisplayName, (l, v) => l.SetDisplayName(v), "Instrument Name"));
            Properties.Add(Create(l => l.ReadLayout().NumberOfStaves, (l, v) => l.SetNumberOfStaves(v), "Number of Staves"));
            Properties.Add(Create(l => l.ReadLayout().Collapsed, (l, v) => l.SetCollapsed(v), "Collapsed"));
        }

        public override string Header => "Instrument Ribbon Properties";
    }

    public class ScoreDocumentPropertiesViewModel : ScoreElementPropertiesViewModel<IScoreDocumentReader, IScoreDocumentEditor>
    {
        internal ScoreDocumentPropertiesViewModel(IEnumerable<IScoreDocumentReader> notes, IScoreBuilder scoreBuilder) : base(scoreBuilder, notes)
        {

        }

        public override string Header => "Score Document Properties";
    }
}
