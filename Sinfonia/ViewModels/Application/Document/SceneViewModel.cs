namespace Sinfonia.ViewModels.Application.Document
{
    public class SceneViewModel : BaseViewModel
    {
        public bool IsActive
        {
            get => GetValue(() => IsActive);
            set => SetValue(() => IsActive, value);
        }
        public string Name =>
            ScoreDocumentScene.Name;
        public IExternalScene ScoreDocumentScene { get; }


        public PropertyCollectionViewModel Settings { get; }



        public SceneViewModel(IExternalScene scoreDocumentScene, PropertyCollectionViewModel sceneSettingsViewModel)
        {
            ScoreDocumentScene = scoreDocumentScene;
            Settings = sceneSettingsViewModel;
        }
    }
}
