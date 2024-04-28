namespace Sinfonia.ViewModels.Base
{
    public abstract class BaseMenuViewModel : BaseViewModel
    {
        public ObservableCollection<MenuItemViewModel> MenuItems { get; }


        public BaseMenuViewModel()
        {
            MenuItems = [];
        }
    }
}
