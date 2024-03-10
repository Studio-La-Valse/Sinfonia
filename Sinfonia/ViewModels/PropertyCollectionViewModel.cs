namespace Sinfonia.ViewModels
{
    public abstract class PropertyCollectionViewModel : BaseViewModel
    {
        public abstract string Header { get; }

        public ObservableCollection<PropertyViewModel> Properties
        {
            get => GetValue(() => Properties);
            set => SetValue(() => Properties, value);
        }

        public PropertyCollectionViewModel()
        {
            Properties = [];
        }
    }
}
