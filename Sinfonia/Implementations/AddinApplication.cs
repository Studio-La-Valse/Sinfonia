using Sinfonia.ViewModels.Application;
using System.Diagnostics.CodeAnalysis;


namespace Sinfonia.Implementations
{
    internal class AddinApplication : IApplication
    {
        private readonly DocumentCollectionViewModel documents;

        public AddinApplication(DocumentCollectionViewModel documents)
        {
            this.documents = documents;
        }

        public IDocument? ActiveDocument()
        {
            DocumentViewModel? viewModel = documents.Documents.FirstOrDefault(d => d.IsActive);
            if (viewModel == null)
            {
                return null;
            }
            AddinDocument addinDocument = new(viewModel);
            return addinDocument;
        }

        public IDocument ActiveDocumentOrThrow()
        {
            DocumentViewModel? viewModel = documents.Documents.FirstOrDefault(d => d.IsActive);
            if (viewModel == null)
            {
                throw new Exception("No document open.");
            }
            AddinDocument addinDocument = new(viewModel);
            return addinDocument;
        }

        public bool TryGetActiveDocument([NotNullWhen(true)] out IDocument? activeDocument)
        {
            activeDocument = null;
            DocumentViewModel? viewModel = documents.Documents.FirstOrDefault(d => d.IsActive);
            if (viewModel == null)
            {
                return false;
            }
            activeDocument = new AddinDocument(viewModel);
            return true;
        }
    }
}
