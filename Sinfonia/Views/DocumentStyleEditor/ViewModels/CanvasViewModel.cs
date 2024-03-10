using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Views.DocumentStyleEditor.ViewModels
{
    public class CanvasViewModel : BaseViewModel
    {
        private readonly IScoreDocumentReader scoreDocumentReader;

        public ScoreDocumentStyleTemplate ScoreDocumentStyle { get; }


        public ObservableBoundingBox SelectionBorder
        {
            get => GetValue(() => SelectionBorder);
            set => SetValue(() => SelectionBorder, value);
        }
        public INotifyEntityChanged<IUniqueScoreElement> Invalidator
        {
            get => GetValue(() => Invalidator);
            set => SetValue(() => Invalidator, value);
        }

        public SceneManager<IUniqueScoreElement, int>? Scene
        {
            get => GetValue(() => Scene);
            set => SetValue(() => Scene, value);
        }

        public bool EnablePan
        {
            get => GetValue(() => EnablePan);
            set => SetValue(() => EnablePan, value);
        }

        public IPipe Pipe
        {
            get => GetValue(() => Pipe);
            set => SetValue(() => Pipe, value);
        }

        public CanvasViewModel(INotifyEntityChanged<IUniqueScoreElement> observable, ObservableBoundingBox observableBoundingBox, IScoreDocumentReader scoreDocumentReader, ScoreDocumentStyleTemplate scoreDocumentStyle)
        {
            this.scoreDocumentReader = scoreDocumentReader;

            ScoreDocumentStyle = scoreDocumentStyle;
            Invalidator = observable;
            EnablePan = true;
            SelectionBorder = observableBoundingBox;
            Pipe = Pipeline.DoNothing();
        }

        public void Rerender()
        {
            Invalidator.Invalidate(scoreDocumentReader);
            Invalidator.RenderChanges();
        }
    }
}
