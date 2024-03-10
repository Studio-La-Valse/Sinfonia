namespace Sinfonia.ViewModels.Application.Menu
{
    public class MenuViewModel : BaseMenuViewModel
    {
        public MenuViewModel(FileMenuViewModel fileMenuViewModel, ViewMenuViewModel viewMenuViewModel)
        {
            MenuItems.Add(fileMenuViewModel);
            MenuItems.Add(viewMenuViewModel);
        }
    }
}
