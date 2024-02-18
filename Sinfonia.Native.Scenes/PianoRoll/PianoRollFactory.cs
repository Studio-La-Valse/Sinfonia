namespace Sinfonia.Native.Scenes.PianoRoll
{
    internal class PianoRollFactory : IVisualScoreDocumentContentFactory
    {
        private readonly ISelection<IUniqueScoreElement> selection;
        private readonly Func<double> noteHeight;

        public PianoRollFactory(ISelection<IUniqueScoreElement> selection, Func<double> noteHeight)
        {
            this.selection = selection;
            this.noteHeight = noteHeight;
        }
        public BaseContentWrapper CreateContent(IScoreDocumentReader scoreDocument)
        {
            return new PianoRoll(scoreDocument, selection, noteHeight);
        }
    }
}
