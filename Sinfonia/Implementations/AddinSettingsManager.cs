namespace Sinfonia.Implementations
{
    internal class AddinSettingsManager : IAddinSettingsManager
    {
        private readonly PropertyCollectionViewModel addinSettingsViewModel;

        public AddinSettingsManager(PropertyCollectionViewModel addinSettingsViewModel)
        {
            this.addinSettingsViewModel = addinSettingsViewModel;
        }

        public void Register<T>(Func<T> getValue, Action<T> setValue, string description)
        {
            PropertyViewModel<T> tunnel = new(getValue, setValue, description);

            addinSettingsViewModel.Properties.Add(tunnel);
        }
    }
}
