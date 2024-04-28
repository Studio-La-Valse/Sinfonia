using Sinfonia.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sinfonia.DataTemplateSelectors
{
    public class PropertyTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if(container is not FrameworkElement element)
            {
                throw new UnreachableException();
            }

            var dataTempalte = item switch
            {
                PropertyViewModel<ColorARGB> => element.FindResource("ColorTemplate"),
                PropertyViewModel<double> => element.FindResource("DoubleTemplate"),
                _ => element.FindResource("DefaultTemplate")
            };

            if(dataTempalte is not DataTemplate d)
            {
                throw new UnreachableException();
            }

            return d;
        }
    }
}
