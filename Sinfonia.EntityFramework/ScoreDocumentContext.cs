using Microsoft.EntityFrameworkCore;
using Sinfonia.EntityFramework.Entities;

namespace Sinfonia.EntityFramework
{
    public class ScoreDocumentContext : DbContext
    {
        public DbSet<ScoreDocumentModel> ScoreDocuments { get; set; }
        public DbSet<ScoreMeasureModel> ScoreMeasures { get; set; }
        public DbSet<InstrumentRibbonModel> InstrumentRibbons { get; set; }
        public DbSet<InstrumentMeasureModel> InstrumentMeasures { get; set; }
        public DbSet<MeasureBlockModel> MeasureBlocks { get; set; }
        public DbSet<ChordModel> Chords { get; set; }
        public DbSet<NoteModel> Notes { get; set; }



        public DbSet<ScoreDocumentLayoutModel> ScoreDocumentLayouts { get; set; }
        public DbSet<ScoreMeasureLayoutModel> ScoreMeasureLayouts { get; set; }
        public DbSet<InstrumentRibbonLayoutModel> InstrumentRibbonLayouts { get; set; }
        public DbSet<InstrumentMeasureLayoutModel> InstrumentMeasureLayouts { get; set; }
        public DbSet<MeasureBlockLayoutModel> MeasureBlockLayouts { get; set; } 
        public DbSet<ChordLayoutModel> ChordLayouts { get; set; }
        public DbSet<NoteLayoutModel> NoteLayouts { get; set; }



        public DbSet<ScoreDocumentStyleTemplateModel> ScoreDocumentStyleTemplates { get; set; }
        public DbSet<PageStyleTemplateModel> PageStyleTemplates { get; set; }
        public DbSet<ScoreMeasureStyleTemplateModel> ScoreMeasureStyleTemplates { get; set; }
        public DbSet<InstrumentMeasureStyleTemplateModel> InstrumentMeasureStyleTemplates { get; set; }
        public DbSet<InstrumentRibbonStyleTemplateModel> InstrumentRibbonStyleTemplates { get; set; }
        public DbSet<ChordStyleTemplateModel> ChordStyleTemplates { get; set; }
        public DbSet<NoteStyleTemplateModel> NoteStyleTemplates { get; set; }
        public DbSet<MeasureBlockStyleTemplateModel> MeasureBlockStyleTemplates { get; set; }
        public DbSet<StaffStyleTemplateModel> StaffStyleTemplates { get; set; }
        public DbSet<StaffGroupStyleTemplateModel> StaffGroupStyleTemplates { get; set; }
        public DbSet<StaffSystemStyleTemplateModel> StaffSystemStyleTemplates { get; set; }



        public ScoreDocumentContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }
    }
}
