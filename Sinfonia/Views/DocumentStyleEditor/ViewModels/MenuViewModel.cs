using Sinfonia.ViewModels.Application.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinfonia.Views.DocumentStyleEditor.ViewModels
{
    public class MenuViewModel : BaseMenuViewModel
    {
        public MenuViewModel(FileMenuViewModel fileMenuViewModel)
        {
            MenuItems.Add(fileMenuViewModel);
        }
    }
}
