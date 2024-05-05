using Sinfonia.ViewModels.Base;

namespace Sinfonia.ViewModels.Application.Menu
{
    public class FileMenuViewModel : MenuItemViewModel
    {
        private readonly DocumentCollectionViewModel documentCollection;
        private readonly IShellMethods shellMethods;

        public FileMenuViewModel(DocumentCollectionViewModel documentCollection, ImportMenuViewModel importMenuViewModel, IShellMethods shellMethods, ICommandFactory commandFactory) : base("_File...")
        {
            this.documentCollection = documentCollection;
            this.shellMethods = shellMethods;

            Items.Add(importMenuViewModel);
            Items.Add(new MenuItemViewModel("Close", commandFactory.Create(CloseDocument, documentCollection.Documents.Any)));
            Items.Add(new MenuItemViewModel("Exit", commandFactory.Create(Exit)));
        }

        public void CloseDocument()
        {
            var openDocument = documentCollection.Documents.ElementAtOrDefault(documentCollection.SelectedIndex);
            if (openDocument is null)
            {
                return;
            }

            documentCollection.Close(openDocument);
        }

        public void Exit()
        {
            for (var i = documentCollection.Documents.Count - 1; i >= 0; i--)
            {
                var document = documentCollection.Documents[i];
                documentCollection.Close(document);
            }

            shellMethods.Exit();
        }
    }
}
