using Sinfonia.ViewModels.Application.Menu;
using Sinfonia.ViewModels.Base;

namespace Sinfonia.ViewModels.Application.Menu
{
    public class MenuViewModel : BaseViewModel
    {
        public ObservableCollection<MenuItemViewModel> Items { get; set; }

        public MenuViewModel(FileMenuViewModel fileMenuViewModel, DocumentMenuViewModel documentMenuViewModel)
        {
            fileMenuViewModel.Icon = new() { Kind = Material.Icons.MaterialIconKind.Menu, Width = 40};
            documentMenuViewModel.Icon = new() { Kind = Material.Icons.MaterialIconKind.Cog, Width = 40 };

            Items = new ObservableCollection<MenuItemViewModel>()
            {
                fileMenuViewModel, documentMenuViewModel
            };
        }
    }
}
