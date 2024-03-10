namespace Sinfonia.ViewModels.Application.Menu
{
    public class ViewMenuViewModel : MenuItemViewModel
    {
        private readonly IDocumentStyleEditorLauncher documentStyleEditorLauncher;

        public ViewMenuViewModel(IDocumentStyleEditorLauncher documentStyleEditorLauncher, ICommandFactory commandFactory) : base("View")
        {
            this.documentStyleEditorLauncher = documentStyleEditorLauncher;

            MenuItems.Add(new MenuItemViewModel("Style Editor", commandFactory.Create(Launch)));
        }

        public void Launch()
        {
            documentStyleEditorLauncher.Launch();
        }
    }
}
