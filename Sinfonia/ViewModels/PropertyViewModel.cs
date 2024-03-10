namespace Sinfonia.ViewModels
{
    public abstract class PropertyViewModel : BaseViewModel
    {
        public string Description
        {
            get => GetValue(() => Description);
            set => SetValue(() => Description, value);
        }


        public PropertyViewModel(string description)
        {
            Description = description;
        }
    }

    public class PropertyViewModel<TProperty> : PropertyViewModel
    {
        private readonly Action<TProperty> setValue;
        private readonly Func<TProperty> getValue;

        public TProperty Value
        {
            get => getValue()!;
            set
            {
                setValue(value);
                NotifyPropertyChanged(nameof(Value));
            }
        }

        public PropertyViewModel(Func<TProperty> getValue, Action<TProperty> setValue, string description) : base(description)
        {
            this.setValue = setValue;
            this.getValue = getValue;
        }
    }
}
