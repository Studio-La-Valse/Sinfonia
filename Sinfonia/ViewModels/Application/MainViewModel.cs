using Sinfonia.ViewModels.Application.Menu;
using Sinfonia.ViewModels.Base;

namespace Sinfonia.ViewModels.Application
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
