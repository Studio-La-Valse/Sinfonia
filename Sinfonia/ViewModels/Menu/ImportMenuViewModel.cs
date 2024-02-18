using Sinfonia.Implementations.ScoreDocument.Factories;
using Sinfonia.ViewModels.Application;
using System.IO;
using System.Xml.Linq;

namespace Sinfonia.ViewModels.Menu
{
    public class ImportMenuViewModel : MenuItemViewModel
    {
        private readonly DocumentCollectionViewModel documentCollectionViewModel;
        private readonly IDocumentViewModelFactory documentViewModelFactory;
        private readonly Interfaces.IBrowseToFile browseToFile;

        public ImportMenuViewModel(ICommandFactory commandFactory, DocumentCollectionViewModel documentCollectionViewModel, IDocumentViewModelFactory documentViewModelFactory, Interfaces.IBrowseToFile browseToFile) : base("Import")
        {
            MenuItems.Add(new MenuItemViewModel("MusicXml...", commandFactory.Create(LoadMusicXml)));
            this.documentCollectionViewModel = documentCollectionViewModel;
            this.documentViewModelFactory = documentViewModelFactory;
            this.browseToFile = browseToFile;
        }

        public void LoadMusicXml()
        {
            if (browseToFile.BrowseToFile(out var filepath))
            {
                using var fileStream = new FileStream(filepath, FileMode.Open);
                var document = XDocument.Load(fileStream);
                var factory = new ScoreBuilderFactoryFromXml(document);
                var documentViewModel = documentViewModelFactory.Create(factory);
                documentCollectionViewModel.Add(documentViewModel);
            }
        }
    }
}
