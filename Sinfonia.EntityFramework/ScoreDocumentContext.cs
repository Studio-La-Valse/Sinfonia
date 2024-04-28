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


        public ScoreDocumentContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }
    }
}
