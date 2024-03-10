using Sinfonia.ViewModels.Application;
using Sinfonia.ViewModels.Application.Menu;

namespace Sinfonia.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MenuViewModel MenuViewModel { get; }
        public DocumentCollectionViewModel DocumentCollectionViewModel { get; }



        public MainViewModel(MenuViewModel menuViewModel, DocumentCollectionViewModel documentCollectionViewModel)
        {
            MenuViewModel = menuViewModel;
            DocumentCollectionViewModel = documentCollectionViewModel;
        }
    }
}
