#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

namespace Sinfonia.EntityFramework.Entities
{
    public class ScoreDocumentLayoutModel
    {
        public Guid Id { get; set; }

        [ForeignKey("ScoreDocument")]
        public Guid ScoreDocumentId { get; set; }
        public ScoreDocumentModel ScoreDocument { get; set; }

        public ICollection<ScoreMeasureLayoutModel> ScoreMeasureLayouts { get; set; }

        public double? Scale { get; set; }
    }

    public class ScoreMeasureLayoutModel
    {
        public Guid Id { get; set; }

        [ForeignKey("ScoreMeasure")]
        public Guid ScoreMeasureId { get; set; }
        public ScoreMeasureModel ScoreMeasure { get; set; }


        public ScoreDocumentLayoutModel ScoreDocumentLayout { get; set; }


        public double? Width { get; set; }
        public double? PaddingRight { get; set; }
        public double? PaddingLeft { get; set; }
        public int? KeySignatureStep { get; set; }
        public int? KeySignatureShift { get; set; }
        public int? KeySignatureMarorOrMinor { get; set; }
    }

    public class InstrumentRibbonLayoutModel
    {
        public Guid Id { get; set; }

        [ForeignKey("InstrumentRibbon")]
        public Guid InstrumentRibbonId { get; set; }
        public InstrumentRibbonModel InstrumentRibbon { get; set; }


        public ScoreDocumentLayoutModel ScoreDocumentLayout { get; set; }


        public string AbbreviatedName { get; set; }
        public string DisplayName { get; set; }
        public int? NumberOfStaves { get; set; }
        public bool Collapsed { get; set; }
    }
}

#nullable enable
