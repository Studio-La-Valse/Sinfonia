#nullable disable

using Sinfonia.EntityFramework.Attributes;

namespace Sinfonia.EntityFramework.Entities
{
    public class ScoreDocumentModel
    {
        public Guid Id { get; set; }

        public ICollection<ScoreMeasureModel> ScoreMeasures { get; set; }

        public ICollection<InstrumentRibbonModel> InstrumentRibbons { get; set; }
    }

    public class ScoreMeasureModel
    {
        public Guid Id { get; set; }

        public ScoreDocumentModel ScoreDocument { get; set; }

        public ICollection<InstrumentMeasureModel> InstrumentMeasures { get; set; }

        public int TimeSignatureNumerator { get; set; }

        public int TimeSignatureDenominator { get; set; }

        public int IndexInScore { get; set; }
    }

    public class InstrumentRibbonModel
    {
        public Guid Id { get; set; }

        public ScoreDocumentModel ScoreDocument { get; set; }

        public ICollection<InstrumentMeasureModel> InstrumentMeasures { get; set; }

        public string Instrument { get; set; }

        public int IndexInScore { get; set; }
    }

    public class InstrumentMeasureModel
    {
        public Guid Id { get; set; }

        public ScoreMeasureModel ScoreMeasure { get; set; }

        public InstrumentRibbonModel InstrumentRibbon { get; set; }

        public ICollection<MeasureBlockModel> MeasureBlocks { get; set; }

        public int ScoreMeasureIndex { get; set; }

        public int InstrumentRibbonIndex { get; set; }
    }

    public class MeasureBlockModel
    {
        public Guid Id { get; set; }

        public ICollection<ChordModel> Chords { get; set; }

        public int Voice { get; set; }

        [PowerOfTwo]
        public int Duration { get; set; }

        public int Dots { get; set; }
    }

    public class ChordModel
    {
        public Guid Id { get; set; }

        public MeasureBlockModel MeasureBlock { get; set; }

        public ICollection<NoteModel> Notes { get; set; }

        [PowerOfTwo]
        public int Duration { get; set; }

        public int Dots { get; set; }
    }

    public class NoteModel
    {
        public Guid Id { get; set; }

        public ChordModel Chord { get; set; }

        public int Octave { get; set; }

        public int Step { get; set; }

        public int Shifts { get; set; }
    }
}

#nullable enable