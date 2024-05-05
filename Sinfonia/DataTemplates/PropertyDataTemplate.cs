using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Media;
using Avalonia.Metadata;
using Sinfonia.ViewModels.Base;
using System.Diagnostics;

namespace Sinfonia.DataTemplates
{
    public class PropertyDataTemplate : IDataTemplate
    {
        [Content]
        public Dictionary<string, IDataTemplate> AvailableTemplates { get; } = new Dictionary<string, IDataTemplate>();

        public Control Build(object? data)
        {
            if (data is null)
            {
                throw new UnreachableException();
            }

            if(data is not PropertyViewModel)
            {
                throw new UnreachableException();
            }

            var template = data switch
            {
                PropertyViewModel<int> => AvailableTemplates["Int"],
                PropertyViewModel<string> => AvailableTemplates["String"],
                PropertyViewModel<double> => AvailableTemplates["Double"],
                PropertyViewModel<Color> => AvailableTemplates["Color"],
                _ => null
            };
            return template?.Build(data) ?? new TextBlock() { Text = $"Unknown element found: {data.GetType()}"};
        }

        public bool Match(object? data)
        {
            return data is PropertyViewModel;
        }
    }
}
