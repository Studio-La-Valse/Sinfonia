using Sinfonia.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinfonia.Implementations;
internal class OptionsWindowService : IOptionsWindowService
{
    private OptionsWindow? optionsWindow;

    public OptionsWindowService()
    {
        
    }

    public void Close()
    {
        if(optionsWindow != null)
        {
            optionsWindow.Close();
        }
    }

    public void Show()
    {
        if(optionsWindow is not null)
        {
            optionsWindow.Activate();
            return;
        }

        optionsWindow = new OptionsWindow();
        optionsWindow.Closed += OptionsWindow_Closed;
        optionsWindow.Show();
    }

    private void OptionsWindow_Closed(object? sender, EventArgs e)
    {
        optionsWindow = null;
    }
}
