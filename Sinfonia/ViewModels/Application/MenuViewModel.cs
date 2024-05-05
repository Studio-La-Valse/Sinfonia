using Sinfonia.ViewModels.Application.Menu;
using Sinfonia.ViewModels.Base;

namespace Sinfonia.ViewModels.Application
{
    public class MenuViewModel : BaseViewModel
    {
        public ObservableCollection<MenuItemViewModel> Items { get; set; }

        public MenuViewModel(FileMenuViewModel fileMenuViewModel, DocumentMenuViewModel documentMenuViewModel)
        {
            Items = new ObservableCollection<MenuItemViewModel>();
            Items.Add(fileMenuViewModel);
            Items.Add(documentMenuViewModel);
        }
    }
}
