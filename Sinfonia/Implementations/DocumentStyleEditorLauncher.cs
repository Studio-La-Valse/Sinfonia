using Sinfonia.Views.DocumentStyleEditor;

namespace Sinfonia.Implementations
{
    internal class DocumentStyleEditorLauncher : IDocumentStyleEditorLauncher
    {
        private readonly DocumentStyleEditorViewFactory documentStyleEditorFactory;
        private DocumentStyleEditorView? documentStyleEditorView;

        public DocumentStyleEditorLauncher(DocumentStyleEditorViewFactory documentStyleEditorFactory)
        {
            this.documentStyleEditorFactory = documentStyleEditorFactory;
        }

        public void Launch()
        {
            if (documentStyleEditorView == null)
            {
                documentStyleEditorView = documentStyleEditorFactory.Create();
                documentStyleEditorView.Closed += DocumentStyleEditorView_Closed;
                documentStyleEditorView.Show();
            }
            else
            {
                documentStyleEditorView.Activate();
            }
        }

        private void DocumentStyleEditorView_Closed(object? sender, EventArgs e)
        {
            documentStyleEditorView = null;
        }
    }
}
