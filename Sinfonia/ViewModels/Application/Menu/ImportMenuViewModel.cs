using Sinfonia.Implementations;
using Sinfonia.ViewModels.Base;
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
        private readonly IScoreDocumentRepository scoreDocumentRepository;

        public ImportMenuViewModel(ICommandFactory commandFactory, DocumentCollectionViewModel documentCollectionViewModel, Interfaces.IBrowseToFile browseToFile, IDocumentViewModelFactory documentViewModelFactory, IScoreDocumentRepository scoreDocumentRepository) : base("Import")
        {
            this.documentCollectionViewModel = documentCollectionViewModel;
            this.browseToFile = browseToFile;
            this.documentViewModelFactory = documentViewModelFactory;
            this.scoreDocumentRepository = scoreDocumentRepository;
            MenuItems.Add(new MenuItemViewModel("MusicXml...", commandFactory.Create(LoadMusicXml)));
            MenuItems.Add(new MenuItemViewModel("From Repository", commandFactory.Create(FromRepository, () => scoreDocumentRepository.AvailableScores().Any())));
        }

        public void LoadMusicXml()
        {
            if (browseToFile.BrowseToFile(".musicxml", "Music XML Files(*.musicxml)|*musicxml|XML Files(*.xml)|*xml", out var filepath))
            {
                using FileStream fileStream = new(filepath, FileMode.Open);
                var document = XDocument.Load(fileStream);
                var memento = new ScoreDocumentMemento()
                {
                    Guid = Guid.NewGuid(),
                    InstrumentRibbons = [],
                    ScoreMeasures = [],
                    Layout = null
                };
                var documentViewModel = documentViewModelFactory.Create(memento);
                var layout = documentViewModel.PageViewLayout;
                _ = documentViewModel.ScoreBuilder.Edit(e =>
                {
                    e.BuildFromXml(layout, document);
                }).Build();
                documentViewModel.Explorer.Rebuild();
                documentCollectionViewModel.Add(documentViewModel);
            }
        }

        public void FromRepository()
        {
            var guid = scoreDocumentRepository.AvailableScores().First();
            var memento = scoreDocumentRepository
                .Get(guid)
                .Chain(scoreDocumentRepository.RestoreLayout);

            var documentViewModel = documentViewModelFactory.Create(memento);
            documentViewModel.Explorer.Rebuild();

            documentCollectionViewModel.Add(documentViewModel);
        }
    }
}
