namespace Sinfonia.ViewModels
{
    public class MenuItemViewModel : BaseViewModel
    {
        public ObservableCollection<MenuItemViewModel?> MenuItems
        {
            get => GetValue(() => MenuItems);
            set => SetValue(() => MenuItems, value);
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

        public MenuItemViewModel(string header)
        {
            Command = null;
            MenuItems = [];
            Header = header;
        }
        public MenuItemViewModel(string header, ICommand command)
        {
            Command = command;
            MenuItems = [];
            Header = header;
        }
        public MenuItemViewModel(string header, ICommand command, object commandParameter)
        {
            Command = command;
            MenuItems = [];
            Header = header;
            CommandParameter = commandParameter;
        }
    }
}
