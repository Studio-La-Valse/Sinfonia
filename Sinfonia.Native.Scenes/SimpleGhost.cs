namespace Sinfonia.Native.Scenes
{
    internal class SimpleGhost : BaseContentWrapper
    {
        private readonly BaseSelectableParent<IUniqueScoreElement> creator;



        public SimpleGhost(BaseSelectableParent<IUniqueScoreElement> creator)
        {
            this.creator = creator;
        }

        public override IEnumerable<BaseContentWrapper> GetContentWrappers()
        {
            return new List<BaseContentWrapper>();
        }

        public override IEnumerable<BaseDrawableElement> GetDrawableElements()
        {
            var alpha = 0;
            if (creator.IsMouseOver)
            {
                alpha += 50;
            }
            if (creator.IsSelected)
            {
                alpha += 100;
            }

            var rectangle = new DrawableRectangle(creator.BoundingBox().Expand(1), new ColorARGB(alpha, 255, 0, 0), cornerRounding: 1);
            return new List<BaseDrawableElement>()
            {
                rectangle
            };
        }
    }
}
