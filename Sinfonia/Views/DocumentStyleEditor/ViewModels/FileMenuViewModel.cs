using StudioLaValse.ScoreDocument.Layout.Templates;
using System.Diagnostics;
using System.IO;
using YamlDotNet.Serialization;
using IBrowseToFile = Sinfonia.Interfaces.IBrowseToFile;

namespace Sinfonia.Views.DocumentStyleEditor.ViewModels
{
    public class FileMenuViewModel : MenuItemViewModel
    {
        private readonly IBrowseToFile browseToFile;
        private readonly DocumentStyleEditorViewModel documentStyleEditorViewModel;
        private readonly CanvasViewModel canvasViewModel;
        private readonly DeserializerBuilder deserializerBuilder;
        private string? loadedFile = null;

        public FileMenuViewModel(ICommandFactory commandFactory, IBrowseToFile browseToFile, DocumentStyleEditorViewModel documentStyleEditorViewModel, CanvasViewModel canvasViewModel, DeserializerBuilder deserializerBuilder) : base("File")
        {
            this.browseToFile = browseToFile;
            this.documentStyleEditorViewModel = documentStyleEditorViewModel;
            this.canvasViewModel = canvasViewModel;
            this.deserializerBuilder = deserializerBuilder;

            MenuItems.Add(new MenuItemViewModel("Load", commandFactory.Create(Load)));
            MenuItems.Add(new MenuItemViewModel("Save", commandFactory.Create(Save, () => loadedFile is not null)));
            MenuItems.Add(new MenuItemViewModel("Save as", commandFactory.Create(SaveAs)));
        }

        public void Load()
        {
            if(browseToFile.BrowseToFile(out var filePath))
            {
                loadedFile = filePath;
                var deserializer = deserializerBuilder.Build();
                using var reader = File.OpenText(filePath);
                var template = deserializer.Deserialize<ScoreDocumentStyleTemplate>(reader);
                canvasViewModel.ScoreDocumentStyle.Apply(template);
                canvasViewModel.Rerender();
                documentStyleEditorViewModel.Refresh();
            }
        }

        public void Save()
        {
            if(loadedFile is null)
            {
                throw new UnreachableException("No file loaded, cannot save. This command should not be accessible if no file is loaded.");
            }
        }

        public void SaveAs()
        {

        }
    }
}
