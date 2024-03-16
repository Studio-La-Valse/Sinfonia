//namespace Sinfonia.Native.Scenes.PianoRoll
//{
//    internal class Measure : BaseSelectableParent<IUniqueScoreElement>
//    {
//        private readonly IScoreMeasureReader scoreMeasure;
//        private readonly ISelection<IUniqueScoreElement> selection;
//        private readonly double canvasLeft;
//        private readonly double measureWidth;
//        private readonly double noteHeight;
//        private readonly double generalSpacing;
//        private readonly ColorARGB gridLineColor;


//        public Measure(IScoreMeasureReader scoreMeasure, ISelection<IUniqueScoreElement> selection, double canvasLeft, double measureWidth, double noteHeight, double generalSpacing, ColorARGB gridLineColor) : base(scoreMeasure, selection)
//        {
//            this.scoreMeasure = scoreMeasure;
//            this.selection = selection;
//            this.canvasLeft = canvasLeft;
//            this.measureWidth = measureWidth;
//            this.noteHeight = noteHeight;
//            this.generalSpacing = generalSpacing;
//            this.gridLineColor = gridLineColor;
//        }




//        public override BoundingBox BoundingBox()
//        {
//            return new BoundingBox(canvasLeft, canvasLeft + measureWidth, 0, 88 * noteHeight);
//        }

//        public override IEnumerable<BaseContentWrapper> GetContentWrappers()
//        {
//            foreach (IInstrumentMeasureReader measure in scoreMeasure.ReadMeasures())
//            {
//                foreach (int voice in measure.ReadVoices())
//                {
//                    IMeasureBlockChainReader blockChain = measure.ReadBlockChainAt(voice);
//                    foreach (IMeasureBlockReader block in blockChain.ReadBlocks())
//                    {
//                        foreach (IChordReader chord in block.ReadChords())
//                        {
//                            foreach (INoteReader note in chord.ReadNotes())
//                            {
//                                double noteCanvasLeft = canvasLeft + ((double)note.Position.Decimal * measureWidth) + (generalSpacing / 2);
//                                NoteBar noteRepresentation = new(note, measure, selection, noteCanvasLeft, measureWidth, noteHeight, generalSpacing, gridLineColor);
//                                yield return noteRepresentation;
//                            }
//                        }
//                    }
//                }
//            }

//            yield return new SimpleGhost(this);
//        }

//        public override IEnumerable<BaseDrawableElement> GetDrawableElements()
//        {
//            return [];
//        }
//    }
//}
