using Sinfonia.ViewModels.Application.Menu;
using Sinfonia.ViewModels.Base;

namespace Sinfonia.ViewModels.Application
{
    public class MainViewModel : BaseViewModel
    {
        public MenuViewModel MenuViewModel { get; }
        public SideBarViewModel SideBarViewModel { get; }
        public DocumentCollectionViewModel DocumentCollectionViewModel { get; }



        public MainViewModel(MenuViewModel menuViewModel, SideBarViewModel sideBarViewModel, DocumentCollectionViewModel documentCollectionViewModel)
        {
            MenuViewModel = menuViewModel;
            SideBarViewModel = sideBarViewModel;
            DocumentCollectionViewModel = documentCollectionViewModel;
        }
    }
}
