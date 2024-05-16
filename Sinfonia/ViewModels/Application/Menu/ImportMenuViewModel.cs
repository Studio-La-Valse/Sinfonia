using Sinfonia.Implementations.ScoreDocument.Memento.Layout;
using Sinfonia.ViewModels.Base;
using StudioLaValse.ScoreDocument.MusicXml;
using System.IO;
using System.Xml.Linq;

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
