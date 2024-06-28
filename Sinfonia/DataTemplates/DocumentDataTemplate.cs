using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Sinfonia.Controls;

namespace Sinfonia.DataTemplates
{
    public class DocumentDataTemplate : IDataTemplate
    {
        private readonly Dictionary<DocumentViewModel, DocumentControl> _cache = new Dictionary<DocumentViewModel, DocumentControl>();
        public Control? Build(object? data)
        {
            if (data is not DocumentViewModel documentViewModel)
            {
                return new TextBlock { Text = "Not Found: " + data?.ToString() };
            }

            if (_cache.TryGetValue(documentViewModel, out var control))
            {
                return control;
            }

            var newControl = new DocumentControl()
            {
                DataContext = documentViewModel
            };

            _cache[documentViewModel] = newControl;
            return newControl;
        }

        public bool Match(object? data)
        {
            return data is DocumentViewModel;
        }
    }
}
