#nullable disable

using StudioLaValse.ScoreDocument.Models.Classes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudioLaValse.ScoreDocument.Models.Entities
{
    public class ScoreDocumentLayoutModel
    {
        public Guid Id { get; set; }


        [ForeignKey("ScoreDocument")]
        public Guid ScoreDocumentId { get; set; }
        public ScoreDocumentModel ScoreDocument { get; set; }


        public List<ScoreMeasureLayoutModel> ScoreMeasureLayouts { get; set; }
        public List<InstrumentRibbonLayoutModel> InstrumentRibbonLayouts { get; set; }


        [Range(0, double.MaxValue)]
        public double? FirstSystemIndent { get; set; }


        [Range(Constants.GenerallyNotZero, double.MaxValue)]
        public double? HorizontalStaffLineThickness { get; set; }


        [Range(Constants.GenerallyNotZero, double.MaxValue)]
        public double? VerticalStaffLineThickness { get; set; }


        [Range(Constants.GenerallyNotZero, double.MaxValue)]
        public double? Scale { get; set; }


        [Range(Constants.GenerallyNotZero, double.MaxValue)]
        public double? StemLineThickness { get; set; }

        public ColorARGB PageColor { get; set; }

        public ColorARGB ForegroundColor { get; set; }

        public Dictionary<Guid, double> InstrumentScales { get; set; }
    }

    public class ScoreMeasureLayoutModel
    {
        public Guid Id { get; set; }

        [ForeignKey("ScoreMeasure")]
        public Guid ScoreMeasureId { get; set; }
        public ScoreMeasureModel ScoreMeasure { get; set; }


        public ScoreDocumentLayoutModel ScoreDocumentLayout { get; set; }
        public List<InstrumentMeasureLayoutModel> InstrumentMeasureLayouts { get; set; }


        [Range(Constants.GenerallyNotZero, double.MaxValue)]
        public double? Width { get; set; }

        [Range(0, double.MaxValue)]
        public double? PaddingRight { get; set; }

        [Range(0, double.MaxValue)]
        public double? PaddingLeft { get; set; }

        [Range(0, double.MaxValue)]
        public double? PaddingBottom { get; set; }

        public KeySignature KeySignature { get; set; }
    }

    public class InstrumentRibbonLayoutModel
    {
        public Guid Id { get; set; }

        [ForeignKey("InstrumentRibbon")]
        public Guid InstrumentRibbonId { get; set; }
        public InstrumentRibbonModel InstrumentRibbon { get; set; }


        public ScoreDocumentLayoutModel ScoreDocumentLayout { get; set; }
        public List<InstrumentMeasureLayoutModel> InstrumentMeasureLayouts { get; set; }


        public string AbbreviatedName { get; set; }

        public string DisplayName { get; set; }

        [Range(1, int.MaxValue)]
        public int? NumberOfStaves { get; set; }

        public bool? Collapsed { get; set; }
    }

    public class InstrumentMeasureLayoutModel
    {
        public Guid Id { get; set; }

        [ForeignKey("InstrumentMeasure")]
        public Guid InstrumentMeasureId { get; set; }
        public InstrumentMeasureModel InstrumentMeasure { get; set; }


        public ScoreDocumentLayoutModel ScoreDocumentLayout { get; set; }
        public InstrumentRibbonLayoutModel InstrumentRibbonLayout { get; set; }
        public ScoreMeasureLayoutModel ScoreMeasureLayout { get; set; }
        public List<MeasureBlockLayoutModel> MeasureBlockLayouts { get; set; }


        public KeySignature KeySignature { get; set; }

        public List<ClefChange> ClefChanges { get; set; }

        public bool? Collapsed { get; set; }

        [Range(1, int.MaxValue)]
        public int? NumberOfStaves { get; set; }

        [Range(0, int.MaxValue)]
        public double? PaddingBottom { get; set; }

        public Dictionary<int, double> StaffPaddingBottom { get; set; }
    }

    public class MeasureBlockLayoutModel
    {
        public Guid Id { get; set; }

        [ForeignKey("MeasureBlock")]
        public Guid MeasureBlockId { get; set; }
        public MeasureBlockModel MeasureBlock { get; set; }


        public ScoreDocumentLayoutModel ScoreDocumentLayout { get; set; }
        public InstrumentMeasureLayoutModel InstrumentMeasureLayout { get; set; }
        public List<ChordLayoutModel> ChordLayouts { get; set; }


        public double? BeamAngle { get; set; }

        public double? StemLength { get; set; }
    }

    public class ChordLayoutModel
    {
        public Guid Id { get; set; }

        [ForeignKey("Chord")]
        public Guid ChordId { get; set; }
        public ChordModel Chord { get; set; }


        public ScoreDocumentLayoutModel ScoreDocumentLayout { get; set; }
        public MeasureBlockLayoutModel MeasureBlockLayout { get; set; }
        public List<NoteLayoutModel> NoteLayouts { get; set; }


        public double? XOffset { get; set; }
    }

    public class NoteLayoutModel
    {
        public Guid Id { get; set; }

        [ForeignKey("Note")]
        public Guid NoteId { get; set; }
        public NoteModel Note { get; set; }


        public ScoreDocumentLayoutModel ScoreDocumentLayout { get; set; }
        public ChordLayoutModel ChordLayout { get; set; }


        public int? ForceAccidental { get; set; }

        [Range(0, int.MaxValue)]
        public int? StaffIndex { get; set; }

        [Range(Constants.GenerallyNotZero, double.MaxValue)]
        public double? Scale { get; set; }

        public double? XOffset { get; set; }
    }
}

#nullable enable
