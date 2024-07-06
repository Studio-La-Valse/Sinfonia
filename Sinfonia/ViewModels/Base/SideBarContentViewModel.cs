using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinfonia.ViewModels.Base;
public abstract class SideBarContentViewModel : BaseViewModel
{
    public abstract string Header { get; }

    public Material.Icons.MaterialIconKind Icon
    {
        get => GetValue(() => Icon);
        set => SetValue(() => Icon, value);
    }

    protected SideBarContentViewModel()
    {
        Icon = Material.Icons.MaterialIconKind.QuestionMark;
    }
}
