using Sinfonia.Implementations;
using Sinfonia.ViewModels.Base;

namespace Sinfonia.ViewModels.Application.Menu
{
    public class FileMenuViewModel : MenuItemViewModel
    {
        private readonly DocumentCollectionViewModel documentCollection;
        private readonly ImportMenuViewModel importMenuViewModel;
        private readonly IShellMethods shellMethods;
        private readonly ICommandFactory commandFactory;
        private readonly IFileSaveService fileService;
        private readonly IDocumentViewModelFactory documentViewModelFactory;

        public FileMenuViewModel(DocumentCollectionViewModel documentCollection,
                                 ImportMenuViewModel importMenuViewModel,
                                 IShellMethods shellMethods,
                                 ICommandFactory commandFactory,
                                 IFileSaveService fileService,
                                 IDocumentViewModelFactory documentViewModelFactory) : base("_File...")
        {
            this.documentCollection = documentCollection;
            this.importMenuViewModel = importMenuViewModel;
            this.shellMethods = shellMethods;
            this.commandFactory = commandFactory;
            this.fileService = fileService;
            this.documentViewModelFactory = documentViewModelFactory;

            Items.Add(new MenuItemViewModel("Open", commandFactory.Create(OpenFile)));
            Items.Add(importMenuViewModel);
            Items.Add(new MenuItemViewModel("Save as...", commandFactory.Create(SaveAs)));
            Items.Add(new MenuItemViewModel("Close", commandFactory.Create(CloseDocument, documentCollection.Documents.Any)));
            Items.Add(new MenuItemViewModel("Exit", commandFactory.Create(Exit)));
        }


        public void OpenFile()
        {
            try
            {
                var (memento, style) = fileService.Get();
                var documentViewModel = documentViewModelFactory.Create(memento);
                documentViewModel.Explorer.Rebuild();
                documentViewModel.CanvasViewModel.ScoreDocumentStyle.Apply(style);
                documentCollection.Add(documentViewModel);
            }
            catch
            {

            }
        }

        public void SaveAs()
        {
            var active = documentCollection.TryGetActiveDocument(out var activeDocument);
            if (!active)
            {
                return;
            }

            fileService.SaveDocument(activeDocument!);
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
