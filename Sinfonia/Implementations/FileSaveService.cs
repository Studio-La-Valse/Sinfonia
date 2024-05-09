using Avalonia.Platform.Storage;
using Sinfonia.Implementations.ScoreDocument.Converters;
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

        public FileSaveService(ScoreDocumentMementoConverter documentMementoConverter, MainWindow mainWindow)
        {
            this.documentMementoConverter = documentMementoConverter;
            this.mainWindow = mainWindow;
        }



        public (ScoreDocumentMemento, ScoreDocumentStyleTemplate) Get()
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
            return (scoreDocument, template);   
        }


        public void SaveDocument(DocumentViewModel documentReader)
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
