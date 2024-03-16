//namespace Sinfonia.Native.Scenes.PianoRoll
//{
//    internal class NoteBar : BaseSelectableParent<IUniqueScoreElement>
//    {
//        private readonly IInstrumentMeasureReader host;
//        private readonly double canvasLeft;
//        private readonly double measureWidth;
//        private readonly double noteHeight;
//        private readonly double generalSpacing;
//        private readonly INoteReader note;




//        public double CanvasTop =>
//            (noteHeight * (88 - note.Pitch.IndexOnKlavier)) + (generalSpacing / 2);
//        public double Width =>
//            MathUtils.Map((double)note.ActualDuration().Decimal, 0, (double)host.TimeSignature.Decimal, 0, measureWidth - generalSpacing);
//        public double Height =>
//            noteHeight - generalSpacing;
//        public static ColorARGB Color => ColorARGB.White;


//        public NoteBar(INoteReader note, IInstrumentMeasureReader host, ISelection<IUniqueScoreElement> selection, double canvasLeft, double measureWidth, double noteHeight, double generalSpacing, ColorARGB gridLineColor) : base(note, selection)
//        {
//            this.note = note;
//            this.host = host;
//            this.canvasLeft = canvasLeft;
//            this.measureWidth = measureWidth;
//            this.noteHeight = noteHeight;
//            this.generalSpacing = generalSpacing;
//        }

//        public override BoundingBox BoundingBox()
//        {
//            return new BoundingBox(canvasLeft, canvasLeft + Width, CanvasTop, CanvasTop + Height);
//        }

//        public override IEnumerable<BaseContentWrapper> GetContentWrappers()
//        {
//            yield return new SimpleGhost(this);
//        }

//        public override IEnumerable<BaseDrawableElement> GetDrawableElements()
//        {
//            yield return new DrawableRectangle(
//                canvasLeft,
//                CanvasTop,
//                Width,
//                Height,
//                color: Color);
//        }
//    }
//}
