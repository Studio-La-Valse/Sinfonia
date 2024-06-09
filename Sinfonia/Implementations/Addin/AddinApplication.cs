using Sinfonia.ViewModels.Application;
using System.Diagnostics.CodeAnalysis;


namespace Sinfonia.Implementations.Addin
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
            IDocument? activeDocument = null;

            if (documents.TryGetActiveDocument(out var document))
            {
                activeDocument = new AddinDocument(document);
            }
            return activeDocument;
        }

        public IDocument ActiveDocumentOrThrow()
        {
            IDocument? activeDocument = null;

            if (documents.TryGetActiveDocument(out var document))
            {
                activeDocument = new AddinDocument(document);
            }
            if (activeDocument == null)
            {
                throw new Exception("No document open.");
            }
            return activeDocument;
        }

        public bool TryGetActiveDocument([NotNullWhen(true)] out IDocument? activeDocument)
        {
            activeDocument = null;

            if(documents.TryGetActiveDocument(out var document))
            {
                activeDocument = new AddinDocument(document);
            }

            return activeDocument is not null;
        }
    }
}
