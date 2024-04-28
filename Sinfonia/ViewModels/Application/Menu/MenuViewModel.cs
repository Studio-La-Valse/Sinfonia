using Sinfonia.ViewModels.Base;

namespace Sinfonia.ViewModels.Application.Menu
{
    public class MenuViewModel : BaseMenuViewModel
    {
        public MenuViewModel(FileMenuViewModel fileMenuViewModel, DocumentMenuViewModel documentMenuViewModel)
        {
            MenuItems.Add(fileMenuViewModel);
            MenuItems.Add(documentMenuViewModel);
        }
    }
}
