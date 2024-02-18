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
            var viewModel = documents.Documents.FirstOrDefault(d => d.IsActive);
            if (viewModel == null)
            {
                return null;
            }
            var addinDocument = new AddinDocument(viewModel);
            return addinDocument;
        }

        public IDocument ActiveDocumentOrThrow()
        {
            var viewModel = documents.Documents.FirstOrDefault(d => d.IsActive);
            if (viewModel == null)
            {
                throw new Exception("No document open.");
            }
            var addinDocument = new AddinDocument(viewModel);
            return addinDocument;
        }

        public bool TryGetActiveDocument([NotNullWhen(true)] out IDocument? activeDocument)
        {
            activeDocument = null;
            var viewModel = documents.Documents.FirstOrDefault(d => d.IsActive);
            if (viewModel == null)
            {
                return false;
            }
            activeDocument = new AddinDocument(viewModel);
            return true;
        }
    }
}
