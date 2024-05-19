using Sinfonia.ViewModels.Base;

namespace Sinfonia.ViewModels.Application.Menu
{
    public class ImportMenuViewModel : MenuItemViewModel
    {
        public ImportMenuViewModel(ICommandFactory commandFactory, IMusicXmlImportService browseToFile) : base("_Import...")
        {
            Items.Add(new MenuItemViewModel("Music Xml...", commandFactory.Create(browseToFile.Import)));
        }
    }
}
