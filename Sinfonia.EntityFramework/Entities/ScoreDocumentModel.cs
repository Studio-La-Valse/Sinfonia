#nullable disable

using StudioLaValse.ScoreDocument.Models.Classes;
using System.ComponentModel.DataAnnotations;

namespace StudioLaValse.ScoreDocument.Models.Entities
{
    public class ScoreDocumentModel
    {
        public Guid Id { get; set; }

        public List<ScoreMeasureModel> ScoreMeasures { get; set; }

        public List<InstrumentRibbonModel> InstrumentRibbons { get; set; }

        public ScoreDocumentLayoutModel Layout { get; set; }
    }

    public class ScoreMeasureModel
    {
        public Guid Id { get; set; }

        public ScoreDocumentModel ScoreDocument { get; set; }

        public List<InstrumentMeasureModel> InstrumentMeasures { get; set; }

        public ScoreMeasureLayoutModel Layout { get; set; }

        public TimeSignature TimeSignature { get; set; }

        [Range(0, int.MaxValue)]
        public int IndexInScore { get; set; }
    }

    public class InstrumentRibbonModel
    {
        public Guid Id { get; set; }

        public ScoreDocumentModel ScoreDocument { get; set; }

        public List<InstrumentMeasureModel> InstrumentMeasures { get; set; }

        public InstrumentRibbonLayoutModel Layout { get; set; }

        public Instrument Instrument { get; set; }

        [Range(0, int.MaxValue)]
        public int IndexInScore { get; set; }
    }

    public class InstrumentMeasureModel
    {
        public Guid Id { get; set; }

        public ScoreMeasureModel ScoreMeasure { get; set; }

        public InstrumentRibbonModel InstrumentRibbon { get; set; }

        public List<MeasureBlockModel> MeasureBlocks { get; set; }

        public InstrumentMeasureLayoutModel Layout { get; set; }


        [Range(0, int.MaxValue)]
        public int ScoreMeasureIndex { get; set; }

        [Range(0, int.MaxValue)]
        public int InstrumentRibbonIndex { get; set; }
    }

    public class MeasureBlockModel
    {
        public Guid Id { get; set; }

        public List<ChordModel> Chords { get; set; }

        public MeasureBlockLayoutModel Layout { get; set; }

        [Range(0, int.MaxValue)]
        public int Voice { get; set; }

        public RythmicDuration Duration { get; set; }
    }

    public class ChordModel
    {
        public Guid Id { get; set; }

        public MeasureBlockModel MeasureBlock { get; set; }

        public List<NoteModel> Notes { get; set; }

        public ChordLayoutModel Layout { get; set; }

        public RythmicDuration RythmicDuration { get; set; }
    }

    public class NoteModel
    {
        public Guid Id { get; set; }

        public ChordModel Chord { get; set; }

        public NoteLayoutModel Layout { get; set; }

        public Pitch Pitch { get; set; }
    }
}

#nullable enable