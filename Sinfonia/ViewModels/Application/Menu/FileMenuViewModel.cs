using Sinfonia.Implementations;
using Sinfonia.ViewModels.Base;

namespace Sinfonia.ViewModels.Application.Menu
{
    public class FileMenuViewModel : MenuItemViewModel
    {
        private readonly DocumentCollectionViewModel documentCollection;
        private readonly IShellMethods shellMethods;
        private readonly IPdfExportService pdfExportService;

        public FileMenuViewModel(DocumentCollectionViewModel documentCollection,
                                 ImportMenuViewModel importMenuViewModel,
                                 IShellMethods shellMethods,
                                 ICommandFactory commandFactory,
                                 IFileSaveService fileService,
                                 IFileSyncService fileSyncService,
                                 IPdfExportService pdfExportService) : base("_File...")
        {
            this.documentCollection = documentCollection;
            this.shellMethods = shellMethods;
            this.pdfExportService = pdfExportService;
            var openItem = new MenuItemViewModel()
            {
                Header = "Open",
                Command = commandFactory.Create(fileService.Open)
            };

            var saveItem = new MenuItemViewModel()
            {
                Header = "Save as...",
                Command = commandFactory.Create(fileService.SaveDocument),
                Icon = new() { Kind = Material.Icons.MaterialIconKind.Floppy }
            };

            var pdfItem = new MenuItemViewModel()
            {
                Header = "Export PDF",
                Command = commandFactory.Create(pdfExportService.Export),
                Icon = new() { Kind = Material.Icons.MaterialIconKind.FilePdfBox }
            };

            var syncItem = new MenuItemViewModel()
            {
                Header = "Sync",
                Command = commandFactory.Create(fileSyncService.UpdloadBorrowed),
                Icon = new() { Kind = Material.Icons.MaterialIconKind.World }
            };

            var closeItem = new MenuItemViewModel()
            {
                Header = "Close",
                Command = commandFactory.Create(CloseDocument)
            };

            var exitItem = new MenuItemViewModel()
            {
                Header = "Exit",
                Command = commandFactory.Create(Exit),
                Icon = new() { Kind = Material.Icons.MaterialIconKind.ExitToApp }
            };
            
            Items.Add(openItem);
            Items.Add(importMenuViewModel);
            Items.Add(saveItem);
            Items.Add(pdfItem);
            Items.Add(syncItem);
            Items.Add(closeItem);
            Items.Add(exitItem);
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
