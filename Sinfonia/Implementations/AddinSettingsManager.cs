namespace Sinfonia.Implementations
{
    internal class AddinSettingsManager : IAddinSettingsManager
    {
        private readonly PropertyManagerViewModel addinSettingsViewModel;

        public AddinSettingsManager(PropertyManagerViewModel addinSettingsViewModel)
        {
            this.addinSettingsViewModel = addinSettingsViewModel;
        }

        public void Register<T>(Func<T> getValue, Action<T> setValue, string description)
        {
            var tunnel = new PropertyViewModel<T>(getValue, setValue, description);

            addinSettingsViewModel.Properties.Add(tunnel);
        }
    }
}
