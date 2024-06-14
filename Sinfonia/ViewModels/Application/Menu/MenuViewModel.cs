using Sinfonia.ViewModels.Application.Menu;
using Sinfonia.ViewModels.Base;

namespace Sinfonia.ViewModels.Application.Menu
{
    public class MenuViewModel : BaseViewModel
    {
        public ObservableCollection<MenuItemViewModel> Items { get; set; }

        public MenuViewModel(FileMenuViewModel fileMenuViewModel, ViewMenuViewModel viewMenuViewModel, IOptionsWindowService optionsWindowService, ICommandFactory commandFactory)
        {
            fileMenuViewModel.Icon = new() { Kind = Material.Icons.MaterialIconKind.Menu, Width = 40};

            var documentMenuViewModel = new MenuItemViewModel()
            {
                Header = "Options",
                Command = commandFactory.Create(optionsWindowService.Show),
                Icon = new() { Kind = Material.Icons.MaterialIconKind.Cog, Width = 40 }
            };

            Items =
            [
                fileMenuViewModel, viewMenuViewModel
            ];
        }
    }
}
