using Sinfonia.Extensions;

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

        public void Add(DocumentViewModel document)
        {
            Documents.Add(document);
            SetActive(document);
        }

        public void SetActive(DocumentViewModel document)
        {
            Documents.ForEach(d => d.SetInactive());
            document.SetActive();

            if (document.AvailableScenes.Count > 0 && !document.AvailableScenes.Any(s => s.IsActive))
            {
                var scene = document.AvailableScenes[0];
                document.ActivateScene(scene);
            }
        }

        public void Close(DocumentViewModel documentViewModel)
        {
            Documents.Remove(documentViewModel);

            if (Documents.Count > 0)
            {
                SetActive(Documents.First());
            }
        }
    }
}
