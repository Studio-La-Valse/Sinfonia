using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.API
{
    public interface IApplication
    {
        IDocument? ActiveDocument();
        IDocument ActiveDocumentOrThrow();
        bool TryGetActiveDocument([NotNullWhen(true)] out IDocument? activeDocument);
    }
}
