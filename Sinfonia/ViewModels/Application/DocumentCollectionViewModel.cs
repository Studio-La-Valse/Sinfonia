using Sinfonia.ViewModels.Base;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.ViewModels.Application
{
    public class DocumentCollectionViewModel : BaseViewModel
    {
        public ICommand SetActiveCommand { get; }
        public ICommand CloseCommand { get; }
        public ObservableCollection<DocumentViewModel> Documents { get; }




        public DocumentCollectionViewModel(ICommandFactory commandFactory)
        {
            Documents = [];
            SetActiveCommand = commandFactory.Create<DocumentViewModel>(SetActive, (d) => true);
            CloseCommand = commandFactory.Create<DocumentViewModel>(Close, (d) => true);
        }

        public DocumentViewModel ActiveDocumentOrThrow()
        {
            return Documents.First(d => d.IsActive);    
        }

        public bool TryGetActiveDocument([NotNullWhen(true)] out DocumentViewModel? activeDocument)
        {
            activeDocument = Documents.FirstOrDefault(d => d.IsActive);

            return activeDocument != null;
        }

        public void Add(DocumentViewModel document)
        {
            Documents.Add(document);
            SetActive(document);
        }

        public void SetActive(DocumentViewModel document)
        {
            Documents.ForEach(d => d.SetInactive());
            document.SetActive();
        }

        public void Close(DocumentViewModel documentViewModel)
        {
            _ = Documents.Remove(documentViewModel);

            if (Documents.Count > 0)
            {
                SetActive(Documents.First());
            }
        }
    }
}
