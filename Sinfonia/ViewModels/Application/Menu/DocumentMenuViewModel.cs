using Sinfonia.EntityFramework;
using Sinfonia.ViewModels.Base;

namespace Sinfonia.ViewModels.Application.Menu
{
    public class DocumentMenuViewModel : MenuItemViewModel
    {
        private readonly DocumentCollectionViewModel documentCollectionViewModel;
        private readonly IScoreDocumentRepository scoreDocumentContext;

        public DocumentMenuViewModel(DocumentCollectionViewModel documentCollectionViewModel, ICommandFactory commandFactory, IScoreDocumentRepository scoreDocumentContext) : base("Document")
        {
            this.documentCollectionViewModel = documentCollectionViewModel;
            this.scoreDocumentContext = scoreDocumentContext;

            Items.Add(new MenuItemViewModel("Upload", commandFactory.Create(Upload, () => documentCollectionViewModel.TryGetActiveDocument(out _))));
        }

        public void Upload()
        {
            var activeDocument = documentCollectionViewModel.Documents.ElementAt(documentCollectionViewModel.SelectedIndex).ScoreDocumentReader;
            scoreDocumentContext.Upload(activeDocument);
        }
    }
}
