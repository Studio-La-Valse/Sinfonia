using Avalonia.Platform.Storage;
using Sinfonia.ViewModels.Application;
using Sinfonia.Windows;
using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.StyleTemplates;
using System.Text.Json;

namespace Sinfonia.Implementations
{
#nullable disable
    public class DocumentModel
    {
        public ScoreDocumentModel ScoreDocument { get; set; }
        public ScoreDocumentLayoutDictionary Layout { get; set; }
    }
#nullable enable

    public class FileSaveService : IFileSaveService
    {
        private readonly JsonSerializerOptions serializerOptions = new()
        {
            
        };
        private readonly MainWindow mainWindow;
        private readonly IDocumentViewModelFactory documentViewModelFactory;
        private readonly DocumentCollectionViewModel documentCollection;

        public FileSaveService(MainWindow mainWindow, IDocumentViewModelFactory documentViewModelFactory, DocumentCollectionViewModel documentCollection)
        {
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
                return;
            }

            var file = result[0];
            using var stream = AsyncHelper.RunSync(file.OpenReadAsync);
            var documentModel = JsonSerializer.Deserialize<DocumentModel>(stream, options: serializerOptions) ?? throw new Exception();
            var documentViewModel = documentViewModelFactory.Create(documentModel.ScoreDocument);
            documentViewModel.Explorer.Rebuild();
            documentCollection.Add(documentViewModel);
        }


        public void SaveDocument()
        {
            if(!documentCollection.TryGetActiveDocument(out var documentReader))
            {
                return;
            }

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
                return;
            }

            using var stream = AsyncHelper.RunSync(result.OpenWriteAsync);
            var documentModel = new DocumentModel()
            {
                ScoreDocument = documentReader.ScoreDocument.Freeze(),
                Layout = documentReader.ScoreDocument.FreezeLayout()
            };
            JsonSerializer.Serialize(stream, documentModel, options: serializerOptions);
        }
    }
}
