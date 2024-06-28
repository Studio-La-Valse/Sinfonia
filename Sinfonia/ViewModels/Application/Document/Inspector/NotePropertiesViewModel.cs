using StudioLaValse.ScoreDocument;

namespace Sinfonia.ViewModels.Application.Document.Inspector
{
    public class NotePropertiesViewModel : ScoreElementPropertiesViewModel<INote>
    {
        internal NotePropertiesViewModel(IEnumerable<INote> notes, IScoreBuilder scoreBuilder) : base(scoreBuilder, notes)
        {
            Properties.Add(Create(l => l.StaffIndex, (l, v) => l.StaffIndex.Value = v, "Staff Index"));
            Properties.Add(Create(l => l.XOffset, (l, v) => l.XOffset.Value = v, "X Offset"));
            Properties.Add(Create(l => l.ForceAccidental, (l, v) => l.ForceAccidental.Value = v, "Accidental"));
            Properties.Add(Create(l => l.Scale, (l, v) => l.Scale.Value = v, "Scale"));
        }

        public override string Header => "Note Properties";
    }

    public class ChordPropertiesViewModel : ScoreElementPropertiesViewModel<IChord>
    {
        internal ChordPropertiesViewModel(IEnumerable<IChord> notes, IScoreBuilder scoreBuilder) : base(scoreBuilder, notes)
        {
            Properties.Add(Create(l => l.XOffset, (l, v) => l.XOffset.Value = v, "X Offset"));
            Properties.Add(Create(l => l.SpaceRight, (l, v) => l.SpaceRight.Value = v, "Space Right"));
        }

        public override string Header => "Chord Properties";
    }

    public class MeasureBlockPropertiesViewModel : ScoreElementPropertiesViewModel<IMeasureBlock>
    {
        internal MeasureBlockPropertiesViewModel(IEnumerable<IMeasureBlock> notes, IScoreBuilder scoreBuilder) : base(scoreBuilder, notes)
        {
            Properties.Add(Create(l => l.StemLength, (l, v) => l.StemLength.Value = v, "Stem Length"));
            Properties.Add(Create(l => l.BeamAngle, (l, v) => l.BeamAngle.Value = v, "Beam Angle"));
        }

        public override string Header => "Measure Block Properties";
    }

    public class InstrumentMeasurePropertiesViewModel : ScoreElementPropertiesViewModel<IInstrumentMeasure>
    {
        internal InstrumentMeasurePropertiesViewModel(IEnumerable<IInstrumentMeasure> notes, IScoreBuilder scoreBuilder) : base(scoreBuilder, notes)
        {

        }

        public override string Header => "Instrument Measure Properties";
    }

    public class ScoreMeasurePropertiesViewModel : ScoreElementPropertiesViewModel<IScoreMeasure>
    {
        internal ScoreMeasurePropertiesViewModel(IEnumerable<IScoreMeasure> notes, IScoreBuilder scoreBuilder) : base(scoreBuilder, notes)
        {
            Properties.Add(Create(l => l.KeySignature, (l, v) => l.KeySignature.Value = v, "Key Signature"));
        }

        public override string Header => "Score Measure Properties";
    }

    public class InstrumentRibbonPropertiesViewModel : ScoreElementPropertiesViewModel<IInstrumentRibbon>
    {
        internal InstrumentRibbonPropertiesViewModel(IEnumerable<IInstrumentRibbon> notes, IScoreBuilder scoreBuilder) : base(scoreBuilder, notes)
        {
            Properties.Add(Create(l => l.AbbreviatedName, (l, v) => l.AbbreviatedName.Value = v, "Nick Name"));
            Properties.Add(Create(l => l.DisplayName, (l, v) => l.DisplayName.Value = v, "Instrument Name"));
            Properties.Add(Create(l => l.NumberOfStaves, (l, v) => l.NumberOfStaves.Value = v, "Number of Staves"));
            Properties.Add(Create(l => l.Collapsed, (l, v) => l.Collapsed.Value = v, "Collapsed"));
        }

        public override string Header => "Instrument Ribbon Properties";
    }

    public class ScoreDocumentPropertiesViewModel : ScoreElementPropertiesViewModel<IScoreDocument>
    {
        internal ScoreDocumentPropertiesViewModel(IEnumerable<IScoreDocument> notes, IScoreBuilder scoreBuilder) : base(scoreBuilder, notes)
        {

        }

        public override string Header => "Score Document Properties";
    }
}
