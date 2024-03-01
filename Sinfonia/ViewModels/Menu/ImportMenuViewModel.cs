using Sinfonia.ViewModels.Application;
using System.IO;
using System.Xml.Linq;
using StudioLaValse.ScoreDocument.MusicXml;

namespace Sinfonia.ViewModels.Menu
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
            if (browseToFile.BrowseToFile(out var filepath))
            {
                using var fileStream = new FileStream(filepath, FileMode.Open);
                var document = XDocument.Load(fileStream);
                var documentViewModel = documentViewModelFactory.Create();
                documentViewModel.ScoreBuilder.BuildFromXml(document);
                documentViewModel.Explorer.Rebuild();
                documentCollectionViewModel.Add(documentViewModel);
            }
        }
    }
}
