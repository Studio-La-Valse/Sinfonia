namespace Sinfonia.ViewModels.Application.Document
{
    public class SceneViewModel : BaseViewModel
    {
        private readonly IExternalScene scoreDocumentScene;



        public bool IsActive
        {
            get => GetValue(() => IsActive);
            set => SetValue(() => IsActive, value);
        }
        public string Name =>
            ScoreDocumentScene.Name;
        public IExternalScene ScoreDocumentScene =>
            scoreDocumentScene;


        public PropertyManagerViewModel Settings { get; }



        public SceneViewModel(IExternalScene scoreDocumentScene, PropertyManagerViewModel sceneSettingsViewModel)
        {
            this.scoreDocumentScene = scoreDocumentScene;
            Settings = sceneSettingsViewModel;
        }
    }
}
