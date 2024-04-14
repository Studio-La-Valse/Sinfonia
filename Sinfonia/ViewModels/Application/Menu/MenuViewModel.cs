namespace Sinfonia.ViewModels.Application.Menu
{
    public class MenuViewModel : BaseMenuViewModel
    {
        public MenuViewModel(FileMenuViewModel fileMenuViewModel)
        {
            MenuItems.Add(fileMenuViewModel);
        }
    }
}
