using Avalonia.Platform.Storage;
using Sinfonia.Windows;
using StudioLaValse.ScoreDocument.Templates;
using System.IO;
using YamlDotNet.Serialization;

namespace Sinfonia.Implementations
{
    public class ScoreStyleTemplateSaveService : IScoreStyleTemplateSaveService
    {
        private readonly MainWindow mainWindow;
        private readonly SerializerBuilder serializerBuilder = new();
        private readonly DeserializerBuilder deserializerBuilder = new();

        public ScoreStyleTemplateSaveService(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public ScoreDocumentStyleTemplate Open()
        {
            var task = mainWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Open",
                AllowMultiple = false,
                FileTypeFilter = [new FilePickerFileType("Yaml files") { Patterns = ["*.yaml", "*.yml"] }]
            });
            var result = AsyncHelper.RunSync(() => task);
            if (result.Count == 0)
            {
                throw new Exception();
            }

            var file = result[0];
            using var fileStream = AsyncHelper.RunSync(file.OpenReadAsync);
            using var reader = new StreamReader(fileStream);
            var template = deserializerBuilder.Build().Deserialize<ScoreDocumentStyleTemplate>(reader);
            return template;
        }

        public void Save(ScoreDocumentStyleTemplate scoreDocumentStyleTemplate)
        {
            var @string = serializerBuilder.Build().Serialize(scoreDocumentStyleTemplate);

            var task = mainWindow.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
            {
                Title = "Save as...",
                DefaultExtension = ".yml",
                SuggestedFileName = "Score Document Style",
                ShowOverwritePrompt = true,
                FileTypeChoices = [new FilePickerFileType("Yaml files") { Patterns = ["*.yaml", "*.yml"] }]
            });
            var result = AsyncHelper.RunSync(() => task);
            if (result is null)
            {
                throw new Exception();
            }

            using var stream = AsyncHelper.RunSync(result.OpenWriteAsync);
            using var textWriter = new StreamWriter(stream);
            textWriter.Write(@string);
        }
    }
}
