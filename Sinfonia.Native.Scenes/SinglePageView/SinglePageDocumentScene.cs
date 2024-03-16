//namespace Sinfonia.Native.Scenes.SinglePageView
//{
//    public class SinglePageDocumentScene : BaseVisualParent<IUniqueScoreElement>
//    {
//        private readonly VisualStaffSystemFactory sceneFactory;
//        private readonly IScoreDocumentReader scoreDocumentReader;
//        private readonly IScoreLayoutProvider scoreLayoutDictionary;
//        private readonly Func<int> page;

//        /// <summary>
//        /// The default constructor. 
//        /// </summary>
//        /// <param name="sceneFactory"></param>
//        /// <param name="scoreDocumentReader"></param>
//        public SinglePageDocumentScene(VisualStaffSystemFactory sceneFactory, IScoreDocumentReader scoreDocumentReader, IScoreLayoutProvider scoreLayoutDictionary, Func<int> page) : base(scoreDocumentReader)
//        {
//            this.sceneFactory = sceneFactory;
//            this.scoreDocumentReader = scoreDocumentReader;
//            this.scoreLayoutDictionary = scoreLayoutDictionary;
//            this.page = page;
//        }

//        /// <inheritdoc/>
//        public override IEnumerable<BaseContentWrapper> GetContentWrappers()
//        {
//            SinglePageViewSceneFactory body = new(page(), sceneFactory, ColorARGB.Black, ColorARGB.White, scoreLayoutDictionary);
//            yield return body.CreateContent(scoreDocumentReader);
//        }

//        /// <inheritdoc/>
//        public override IEnumerable<BaseDrawableElement> GetDrawableElements()
//        {
//            return [];
//        }
//    }
//}
