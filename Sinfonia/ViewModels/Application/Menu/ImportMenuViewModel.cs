using Sinfonia.Implementations.ScoreDocument.Memento.Layout;
using Sinfonia.ViewModels.Base;
using StudioLaValse.ScoreDocument.MusicXml;
using System.IO;
using System.Xml.Linq;

namespace Sinfonia.ViewModels.Application.Menu
{
    public class ImportMenuViewModel : MenuItemViewModel
    {
        private readonly DocumentCollectionViewModel documentCollectionViewModel;
        private readonly IBrowseToFile browseToFile;
        private readonly IDocumentViewModelFactory documentViewModelFactory;



        public ImportMenuViewModel(ICommandFactory commandFactory, DocumentCollectionViewModel documentCollectionViewModel, IBrowseToFile browseToFile, IDocumentViewModelFactory documentViewModelFactory) : base("_Import...")
        {
            this.documentCollectionViewModel = documentCollectionViewModel;
            this.browseToFile = browseToFile;
            this.documentViewModelFactory = documentViewModelFactory;

            Items.Add(new MenuItemViewModel("Music Xml...", commandFactory.Create(LoadMusicXml)));
        }

        public void LoadMusicXml()
        {
            if (browseToFile.BrowseToFile(".musicxml", "Music XML Files(*.musicxml)|*musicxml|XML Files(*.xml)|*xml", out var filepath))
            {
                using FileStream fileStream = new(filepath, FileMode.Open);
                var document = XDocument.Load(fileStream);
                var memento = ScoreDocumentMemento.Create();
                var documentViewModel = documentViewModelFactory.Create(memento);
                _ = documentViewModel.ScoreBuilder.Edit(e =>
                {
                    e.BuildFromXml(document);
                }).Build();
                documentViewModel.Explorer.Rebuild();
                documentCollectionViewModel.Add(documentViewModel);
            }
        }
    }
}
