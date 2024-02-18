using Sinfonia.ViewModels.Application;

namespace Sinfonia.ViewModels.Menu
{
    public class MenuViewModel : BaseViewModel
    {
        public ObservableCollection<MenuItemViewModel> MenuLayout { get; }


        public MenuViewModel(DocumentCollectionViewModel documentCollectionViewModel, FileMenuViewModel fileMenuViewModel, IShellMethods shellMethods, ICommandFactory commandFactory)
        {
            MenuLayout = [];
            MenuLayout.Add(fileMenuViewModel);
        }
    }
}
