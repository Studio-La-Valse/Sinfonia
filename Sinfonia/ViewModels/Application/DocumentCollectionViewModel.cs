using Sinfonia.ViewModels.Base;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.ViewModels.Application
{
    public class DocumentCollectionViewModel : BaseViewModel
    {
        public ObservableCollection<DocumentViewModel> Documents { get; }


        public int SelectedIndex
        {
            get => GetValue(() =>  SelectedIndex);
            set => SetValue(() => SelectedIndex, value);
        }


        public DocumentCollectionViewModel()
        {
            Documents = [];
        }


        public bool TryGetActiveDocument([NotNullWhen(true)] out DocumentViewModel? activeDocument)
        {
            activeDocument = Documents.ElementAtOrDefault(SelectedIndex);

            return activeDocument != null;
        }

        public void Close(DocumentViewModel documentViewModel)
        {
            _ = Documents.Remove(documentViewModel);
        }

        public void Add(DocumentViewModel documentViewModel)
        {
            var newIndex = Documents.Count;
            Documents.Add(documentViewModel);
            SelectedIndex = newIndex;
        }
    }
}
