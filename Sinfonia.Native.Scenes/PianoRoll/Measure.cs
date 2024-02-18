namespace Sinfonia.Native.Scenes.PianoRoll
{
    internal class Measure : BaseSelectableParent<IUniqueScoreElement>
    {
        private readonly IScoreMeasureReader scoreMeasure;
        private readonly ISelection<IUniqueScoreElement> selection;
        private readonly double canvasLeft;
        private readonly double measureWidth;
        private readonly double noteHeight;
        private readonly double generalSpacing;
        private readonly ColorARGB gridLineColor;


        public Measure(IScoreMeasureReader scoreMeasure, ISelection<IUniqueScoreElement> selection, double canvasLeft, double measureWidth, double noteHeight, double generalSpacing, ColorARGB gridLineColor) : base(scoreMeasure, selection)
        {
            this.scoreMeasure = scoreMeasure;
            this.selection = selection;
            this.canvasLeft = canvasLeft;
            this.measureWidth = measureWidth;
            this.noteHeight = noteHeight;
            this.generalSpacing = generalSpacing;
            this.gridLineColor = gridLineColor;
        }




        public override BoundingBox BoundingBox()
        {
            return new BoundingBox(canvasLeft, canvasLeft + measureWidth, 0, 88 * noteHeight);
        }

        public override IEnumerable<BaseContentWrapper> GetContentWrappers()
        {
            foreach (var measure in scoreMeasure.ReadMeasures())
            {
                foreach (var voice in measure.EnumerateVoices())
                {
                    var blockChain = measure.ReadBlockChainAt(voice);
                    foreach (var block in blockChain.ReadBlocks())
                    {
                        foreach (var chord in block.ReadChords())
                        {
                            foreach (var note in chord.ReadNotes())
                            {
                                var noteCanvasLeft = canvasLeft + (double)note.Position.Decimal * measureWidth + generalSpacing / 2;
                                var noteRepresentation = new NoteBar(note, measure, selection, noteCanvasLeft, measureWidth, noteHeight, generalSpacing, gridLineColor);
                                yield return noteRepresentation;
                            }
                        }
                    }
                }
            }

            yield return new SimpleGhost(this);
        }

        public override IEnumerable<BaseDrawableElement> GetDrawableElements()
        {
            return new List<BaseDrawableElement>();
        }
    }
}
