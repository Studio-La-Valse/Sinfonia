using Sinfonia.ViewModels.Base;

namespace Sinfonia.ViewModels.Application.Menu
{
    public class DocumentMenuViewModel : MenuItemViewModel
    {
        private readonly DocumentCollectionViewModel documentCollectionViewModel;
        private readonly IFileSaveService scoreDocumentContext;

        public DocumentMenuViewModel(DocumentCollectionViewModel documentCollectionViewModel, ICommandFactory commandFactory, IFileSaveService scoreDocumentContext) : base("Document")
        {
            this.documentCollectionViewModel = documentCollectionViewModel;
            this.scoreDocumentContext = scoreDocumentContext;
        }
    }
}
