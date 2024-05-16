using Avalonia.Platform.Storage;
using Avalonia.Styling;
using Sinfonia.Implementations.ScoreDocument.Converters;
using Sinfonia.Interfaces;
using Sinfonia.ViewModels.Application;
using Sinfonia.Windows;
using StudioLaValse.ScoreDocument.Layout.Templates;
using StudioLaValse.ScoreDocument.Models.Entities;
using StudioLaValse.ScoreDocument.Reader;
using System.Text.Json;
using System.Xml.Serialization;

namespace Sinfonia.Implementations
{
#nullable disable
    public class DocumentModel
    {
        public ScoreDocumentModel ScoreDocument { get; set; }
        public ScoreDocumentStyleTemplate StyleTemplate { get; set; }
    }
#nullable enable

    public class FileSaveService : IFileSaveService
    {
        private readonly ScoreDocumentMementoConverter documentMementoConverter;
        private readonly MainWindow mainWindow;
        private readonly IDocumentViewModelFactory documentViewModelFactory;
        private readonly DocumentCollectionViewModel documentCollection;

        public FileSaveService(ScoreDocumentMementoConverter documentMementoConverter, MainWindow mainWindow, IDocumentViewModelFactory documentViewModelFactory, DocumentCollectionViewModel documentCollection)
        {
            this.documentMementoConverter = documentMementoConverter;
            this.mainWindow = mainWindow;
            this.documentViewModelFactory = documentViewModelFactory;
            this.documentCollection = documentCollection;
        }



        public void Open()
        {
            var task = mainWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Open",
                AllowMultiple = false,
                FileTypeFilter = [new FilePickerFileType("Sinfonia files") { Patterns = ["*.sin"] }]
            });
            var result = AsyncHelper.RunSync(() => task);
            if(result.Count == 0)
            {
                throw new Exception();
            }

            var file = result[0];
            using var stream = AsyncHelper.RunSync(file.OpenReadAsync);
            var documentModel = JsonSerializer.Deserialize<DocumentModel>(stream) ?? throw new Exception();
            var scoreDocument = documentMementoConverter.Convert(documentModel.ScoreDocument);
            var template = documentModel.StyleTemplate;
            var documentViewModel = documentViewModelFactory.Create(scoreDocument);
            documentViewModel.Explorer.Rebuild();
            documentViewModel.CanvasViewModel.ScoreDocumentStyle.Apply(documentModel.StyleTemplate);
            documentCollection.Add(documentViewModel);
        }


        public void SaveDocument()
        {
            if(documentCollection.TryGetActiveDocument(out var documentReader))
            {
                var task = mainWindow.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
                {
                    Title = "Save as...",
                    DefaultExtension = ".sin",
                    SuggestedFileName = documentReader.Header,
                    ShowOverwritePrompt = true,
                    FileTypeChoices = [new FilePickerFileType("Sinfonia files") { Patterns = ["*.sin"] }]
                });
                var result = AsyncHelper.RunSync(() => task);
                if (result is null)
                {
                    throw new Exception();
                }

                using var stream = AsyncHelper.RunSync(result.OpenWriteAsync);
                var _document = documentMementoConverter.ConvertBack(documentReader.ScoreDocumentCore.GetMemento());
                var documentModel = new DocumentModel()
                {
                    ScoreDocument = _document,
                    StyleTemplate = documentReader.CanvasViewModel.ScoreDocumentStyle
                };
                JsonSerializer.Serialize(stream, documentModel);
            }
        }
    }
}
