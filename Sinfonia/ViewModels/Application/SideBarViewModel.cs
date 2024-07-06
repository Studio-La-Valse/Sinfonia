using Avalonia.Controls;
using Sinfonia.ViewModels.Base;

namespace Sinfonia.ViewModels.Application;
public class SideBarViewModel : BaseViewModel
{
    public SideBarContentViewModel ActiveSideBar
    {
        get => GetValue(() => ActiveSideBar);
        set 
        { 
            SetValue(() => ActiveSideBar, value); 
            if(ContentWidth.Value < 61)
            {
                ContentWidth = new GridLength(400);
            }
        }
    }

    public ObservableCollection<SideBarContentViewModel> SideBars
    {
        get => GetValue(() => SideBars);
        set => SetValue(() => SideBars, value);
    }

    public ICommand TriggerPaneCommand
    {
        get => GetValue(() => TriggerPaneCommand);
        set => SetValue(() => TriggerPaneCommand, value);
    }

    public GridLength ContentWidth
    {
        get => GetValue(() => ContentWidth);
        set => SetValue(() => ContentWidth, value);
    }

    public SideBarViewModel(ICommandFactory commandFactory, DocumentStyleEditorViewModel documentStyleEditorViewModel, UserAccountViewModel userAccountViewModel)
    {
        SideBars =
        [
            documentStyleEditorViewModel,
            userAccountViewModel
        ];

        ActiveSideBar = SideBars[0];

        TriggerPaneCommand = commandFactory.Create(TriggerPane);

        ContentWidth = new GridLength(60);
    }

    public void TriggerPane()
    {
        ContentWidth = ContentWidth.Value < 61 ?
            new GridLength(400) : new GridLength(60);
    }
}
