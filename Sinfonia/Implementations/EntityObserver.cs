using StudioLaValse.Drawable.BitmapPainters;

namespace Sinfonia.Implementations
{
    internal class EntityObserver : IObserver<IUniqueScoreElement>
    {
        public EntityObserver(Func<SceneManager<IUniqueScoreElement, int>?> sceneManager, BaseBitmapPainter baseBitmapPainter)
        {
            SceneManager = sceneManager;
            BaseBitmapPainter = baseBitmapPainter;
        }

        public Func<SceneManager<IUniqueScoreElement, int>?> SceneManager { get; }
        public BaseBitmapPainter BaseBitmapPainter { get; }

        public void OnCompleted()
        {
            var sceneManager = SceneManager();

            if (sceneManager is null)
            {
                throw new Exception("No scenemanager active to invalidate this element.");
            }

            sceneManager.RenderChanges(BaseBitmapPainter);
        }
        public void OnError(Exception error)
        {
            throw error;
        }
        public void OnNext(IUniqueScoreElement value)
        {
            var sceneManager = SceneManager();

            if (sceneManager is null)
            {
                throw new Exception("No scenemanager active to invalidate this element.");
            }

            sceneManager.AddToQueue(value);
        }
    }
}
