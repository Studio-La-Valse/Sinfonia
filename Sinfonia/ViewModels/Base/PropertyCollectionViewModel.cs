namespace Sinfonia.ViewModels.Base
{
    public abstract class PropertyCollectionViewModel : BaseViewModel
    {
        public abstract string Header { get; }

        public bool IsExpanded
        {
            get => GetValue(() => IsExpanded);
            set => SetValue(() => IsExpanded, value);
        }

        public ObservableCollection<PropertyViewModel> Properties
        {
            get => GetValue(() => Properties);
            set => SetValue(() => Properties, value);
        }

        public PropertyCollectionViewModel()
        {
            Properties = [];
            IsExpanded = true;
        }
    }
}
