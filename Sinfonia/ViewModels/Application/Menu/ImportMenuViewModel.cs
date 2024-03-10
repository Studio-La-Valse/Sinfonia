using Sinfonia.ViewModels.Application;
using StudioLaValse.ScoreDocument.MusicXml;
using System.IO;
using System.Xml.Linq;

namespace Sinfonia.ViewModels.Application.Menu
{
    public class ImportMenuViewModel : MenuItemViewModel
    {
        private readonly DocumentCollectionViewModel documentCollectionViewModel;
        private readonly Interfaces.IBrowseToFile browseToFile;
        private readonly IDocumentViewModelFactory documentViewModelFactory;

        public ImportMenuViewModel(ICommandFactory commandFactory, DocumentCollectionViewModel documentCollectionViewModel, Interfaces.IBrowseToFile browseToFile, IDocumentViewModelFactory documentViewModelFactory) : base("Import")
        {
            MenuItems.Add(new MenuItemViewModel("MusicXml...", commandFactory.Create(LoadMusicXml)));
            this.documentCollectionViewModel = documentCollectionViewModel;
            this.browseToFile = browseToFile;
            this.documentViewModelFactory = documentViewModelFactory;
        }

        public void LoadMusicXml()
        {
            if (browseToFile.BrowseToFile(out string? filepath))
            {
                using FileStream fileStream = new(filepath, FileMode.Open);
                XDocument document = XDocument.Load(fileStream);
                DocumentViewModel documentViewModel = documentViewModelFactory.Create();
                _ = documentViewModel.ScoreBuilder.BuildFromXml(document);
                documentViewModel.Explorer.Rebuild();
                documentCollectionViewModel.Add(documentViewModel);
            }
        }
    }
}
