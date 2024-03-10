using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinfonia.Views.DocumentStyleEditor.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public CanvasViewModel CanvasViewModel { get; }
        public MenuViewModel MenuViewModel { get; }
        public DocumentStyleEditorViewModel DocumentStyleEditorViewModel { get; }

        public MainViewModel(CanvasViewModel canvasViewModel, MenuViewModel menuViewModel, DocumentStyleEditorViewModel documentStyleEditorViewModel)
        {
            CanvasViewModel = canvasViewModel;
            MenuViewModel = menuViewModel;
            DocumentStyleEditorViewModel = documentStyleEditorViewModel;
        }

    }
}
