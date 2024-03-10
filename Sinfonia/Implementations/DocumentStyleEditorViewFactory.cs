using Microsoft.Extensions.DependencyInjection;
using Sinfonia.Views.DocumentStyleEditor;

namespace Sinfonia.Implementations
{
    internal class DocumentStyleEditorViewFactory
    {
        private readonly IServiceProvider serviceProvider;

        public DocumentStyleEditorViewFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public DocumentStyleEditorView Create()
        {
            return serviceProvider.GetRequiredService<DocumentStyleEditorView>();   
        }
    }
}
