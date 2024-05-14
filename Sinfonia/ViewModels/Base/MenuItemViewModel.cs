using YamlDotNet.Core.Tokens;

namespace Sinfonia.ViewModels.Base
{
    public class MenuItemViewModel : BaseViewModel
    {
        public ObservableCollection<MenuItemViewModel?> Items
        {
            get => GetValue(() => Items);
            set => SetValue(() => Items, value);
        }

        public Material.Icons.Avalonia.MaterialIcon Icon 
        {
            get => GetValue(() => Icon);
            set => SetValue(() => Icon, value);
        }

        public string Header
        {
            get => GetValue(() => Header);
            set => SetValue(() => Header, value);
        }

        public ICommand? Command
        {
            get => GetValue(() => Command);
            set => SetValue(() => Command, value);
        }

        public object? CommandParameter
        {
            get => GetValue(() => CommandParameter);
            set => SetValue(() => CommandParameter, value);
        }

        public bool IsEnabled
        {
            get => GetValue(() =>  IsEnabled);
            set => SetValue(() => IsEnabled, value);
        }

        public MenuItemViewModel(string header)
        {
            Command = null;
            Items = [];
            Header = header;
            IsEnabled = true;
        }
        public MenuItemViewModel(string header, ICommand command)
        {
            Command = command;
            Items = [];
            Header = header;
            IsEnabled = true;
        }
        public MenuItemViewModel(string header, ICommand command, object commandParameter)
        {
            Command = command;
            Items = [];
            Header = header;
            CommandParameter = commandParameter;
            IsEnabled = true;
        }
    }
}
