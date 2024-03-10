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
            return [];
        }

        public override IEnumerable<BaseDrawableElement> GetDrawableElements()
        {
            int alpha = 0;
            if (creator.IsMouseOver)
            {
                alpha += 50;
            }
            if (creator.IsSelected)
            {
                alpha += 100;
            }

            DrawableRectangle rectangle = new(creator.BoundingBox().Expand(1), new ColorARGB(alpha, 255, 0, 0), cornerRounding: 1);
            return
            [
                rectangle
            ];
        }
    }
}
