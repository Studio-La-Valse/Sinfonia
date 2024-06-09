using Avalonia.Media;

namespace Sinfonia.ViewModels.Base
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

    public class PropertyViewModelDouble : PropertyViewModel<double>
    {
        public PropertyViewModelDouble(Func<double> getValue, Action<double> setValue, string description) : base(getValue, setValue, description)
        {

        }
    }

    public class PropertyViewModelInt : PropertyViewModel<int>
    {
        public PropertyViewModelInt(Func<int> getValue, Action<int> setValue, string description) : base(getValue, setValue, description)
        {

        }
    }

    public class PropertyViewModelString : PropertyViewModel<string>
    {
        public PropertyViewModelString(Func<string> getValue, Action<string> setValue, string description) : base(getValue, setValue, description)
        {

        }
    }


    public class PropertyViewModelColor : PropertyViewModel<Color>
    {
        public PropertyViewModelColor(Func<Color> getValue, Action<Color> setValue, string description) : base(getValue, setValue, description)
        {

        }
    }
}
