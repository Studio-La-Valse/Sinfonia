namespace Sinfonia.Native.Scenes.SinglePageView
{
    public class SinglePageDocumentScene : BaseVisualParent<IUniqueScoreElement>
    {
        private readonly VisualStaffSystemFactory sceneFactory;
        private readonly IScoreDocumentReader scoreDocumentReader;
        private readonly Func<int> page;

        /// <summary>
        /// The default constructor. 
        /// </summary>
        /// <param name="sceneFactory"></param>
        /// <param name="scoreDocumentReader"></param>
        public SinglePageDocumentScene(VisualStaffSystemFactory sceneFactory, IScoreDocumentReader scoreDocumentReader, Func<int> page) : base(scoreDocumentReader)
        {
            this.sceneFactory = sceneFactory;
            this.scoreDocumentReader = scoreDocumentReader;
            this.page = page;
        }

        /// <inheritdoc/>
        public override IEnumerable<BaseContentWrapper> GetContentWrappers()
        {
            var body = new SinglePageViewSceneFactory(page(), PageSize.A4, sceneFactory, ColorARGB.Black, ColorARGB.White);
            yield return body.CreateContent(scoreDocumentReader);
        }

        /// <inheritdoc/>
        public override IEnumerable<BaseDrawableElement> GetDrawableElements()
        {
            return new List<BaseDrawableElement>();
        }
    }
}
