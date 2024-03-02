﻿namespace Sinfonia.Native.Scenes.PianoRoll
{
    internal class PianoRoll : BaseVisualParent<IUniqueScoreElement>
    {
        private readonly Func<double> noteHeight;
        private static readonly double generalSpacing = 0.2;
        private static readonly ColorARGB gridLineColor = new ColorARGB(100, 255, 255, 255);

        private readonly IScoreDocumentReader score;
        private readonly ISelection<IUniqueScoreElement> selection;
        private readonly IScoreLayoutProvider scoreLayoutDictionary;

        public PianoRoll(IScoreDocumentReader score, ISelection<IUniqueScoreElement> selection, IScoreLayoutProvider scoreLayoutDictionary, Func<double> noteHeight) : base(score)
        {
            this.score = score;
            this.selection = selection;
            this.scoreLayoutDictionary = scoreLayoutDictionary;
            this.noteHeight = noteHeight;
        }


        public override IEnumerable<BaseContentWrapper> GetContentWrappers()
        {
            var canvasLeft = 0d;
            foreach (var scoreMeasure in score.ReadScoreMeasures())
            {
                var layout = scoreLayoutDictionary.ScoreMeasureLayout(scoreMeasure);
                var measureWidth = layout.Width;
                var measure = new Measure(scoreMeasure, selection, canvasLeft, measureWidth, noteHeight(), generalSpacing, gridLineColor);
                yield return measure;

                canvasLeft += measureWidth;
            }
        }

        public override IEnumerable<BaseDrawableElement> GetDrawableElements()
        {
            var canvasLeft = 0d;
            foreach (var scoreMeasure in score.ReadScoreMeasures())
            {
                var layout = scoreLayoutDictionary.ScoreMeasureLayout(scoreMeasure);
                var measureWidth = layout.Width;
                yield return new DrawableLineVertical(canvasLeft, 0, 88 * noteHeight(), 0.2, gridLineColor);

                canvasLeft += measureWidth;
            }

            for (int i = 0; i <= 88; i++)
            {
                yield return (new DrawableLineHorizontal(i * noteHeight(), 0, canvasLeft, 0.2, gridLineColor));
            }
        }
    }
}
