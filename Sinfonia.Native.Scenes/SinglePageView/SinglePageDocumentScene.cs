using StudioLaValse.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.ScoreElements;

namespace Sinfonia.Native.Scenes.SinglePageView
{
    public class SinglePageDocumentScene : BaseVisualParent<IUniqueScoreElement>
    {
        private readonly VisualStaffSystemFactory sceneFactory;
        private readonly IScoreDocumentReader scoreDocumentReader;
        private readonly IScoreLayoutDictionary scoreLayoutDictionary;
        private readonly Func<int> page;

        /// <summary>
        /// The default constructor. 
        /// </summary>
        /// <param name="sceneFactory"></param>
        /// <param name="scoreDocumentReader"></param>
        public SinglePageDocumentScene(VisualStaffSystemFactory sceneFactory, IScoreDocumentReader scoreDocumentReader, IScoreLayoutDictionary scoreLayoutDictionary, Func<int> page) : base(scoreDocumentReader)
        {
            this.sceneFactory = sceneFactory;
            this.scoreDocumentReader = scoreDocumentReader;
            this.scoreLayoutDictionary = scoreLayoutDictionary;
            this.page = page;
        }

        /// <inheritdoc/>
        public override IEnumerable<BaseContentWrapper> GetContentWrappers()
        {
            var body = new SinglePageViewSceneFactory(page(), sceneFactory, ColorARGB.Black, ColorARGB.White, scoreLayoutDictionary);
            yield return body.CreateContent(scoreDocumentReader);
        }

        /// <inheritdoc/>
        public override IEnumerable<BaseDrawableElement> GetDrawableElements()
        {
            return new List<BaseDrawableElement>();
        }
    }
}
