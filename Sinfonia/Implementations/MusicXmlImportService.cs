using Avalonia.Platform.Storage;
using Sinfonia.ViewModels.Application;
using Sinfonia.Windows;
using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.MusicXml;
using System.Xml.Linq;

namespace Sinfonia.Implementations
{
    public class MusicXmlImportService : IMusicXmlImportService
    {
        private readonly MainWindow mainWindow;
        private readonly IDocumentViewModelFactory documentViewModelFactory;
        private readonly DocumentCollectionViewModel documentCollectionViewModel;

        public MusicXmlImportService(MainWindow mainWindow, IDocumentViewModelFactory documentViewModelFactory, DocumentCollectionViewModel documentCollectionViewModel)
        {
            this.mainWindow = mainWindow;
            this.documentViewModelFactory = documentViewModelFactory;
            this.documentCollectionViewModel = documentCollectionViewModel;
        }

        public void Import()
        {
            var task = mainWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Open",
                AllowMultiple = false,
                FileTypeFilter = [new FilePickerFileType("MusicXML Files") { Patterns = ["*.musicxml", "*.xml"] }]
            });
            var result = AsyncHelper.RunSync(() => task);
            if (result.Count == 0)
            {
                return;
            }

            var file = result[0];
            using var fileStream = AsyncHelper.RunSync(file.OpenReadAsync);
            var document = XDocument.Load(fileStream);
            var memento = new ScoreDocumentModel()
            {
                Id = Guid.NewGuid(),
                InstrumentRibbons = [],
                ScoreMeasures = [],
            };
            var metaData = new ScoreDocumentMetaDataModel()
            {
                CreationDate = DateTime.Now,
                LastEditDate = DateTime.Now,
                ScoreDocumentId = memento.Id,
                ComposerFullName = "",
                CompositionMonth = DateTime.Now.Month,
                CompositionYear = DateTime.Now.Year,
                CompositionYearStart = DateTime.Now.Year,
                IsPublic = false,
                Subtitle = "",
                Title = "Music Xml Document",
            };
            var documentViewModel = documentViewModelFactory.Create(memento, metaData);
            _ = documentViewModel.ScoreBuilder.Edit(e =>
            {
                e.BuildFromXml(document);
            }).Build();
            documentViewModel.Explorer.Rebuild();
            documentCollectionViewModel.Add(documentViewModel);
        }
    }
}
