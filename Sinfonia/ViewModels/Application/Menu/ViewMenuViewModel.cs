using Sinfonia.ViewModels.Base;

namespace Sinfonia.ViewModels.Application.Menu
{
    public class ViewMenuViewModel : MenuItemViewModel
    {
        private readonly DocumentCollectionViewModel documentCollectionViewModel;

        public ViewMenuViewModel(DocumentCollectionViewModel documentCollectionViewModel, ICommandFactory commandFactory) : base("View")
        {
            this.documentCollectionViewModel = documentCollectionViewModel;

            Items.Add(new MenuItemViewModel("Zoom", commandFactory.Create(ZoomFirst)));
        }

        public void ZoomFirst()
        {
            if(!documentCollectionViewModel.TryGetActiveDocument(out var activeDocument))
            {
                return;
            }

            var canvas = activeDocument.CanvasViewModel;
            canvas.ZoomFirst();
        }
    }
}
