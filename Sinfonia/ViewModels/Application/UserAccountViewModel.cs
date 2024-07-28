using ReactiveUI;
using Sinfonia.ViewModels.Base;
using System.Threading.Tasks;

namespace Sinfonia.ViewModels.Application;
public class UserAccountViewModel : SideBarContentViewModel
{
    private readonly IFileSyncService fileSyncService;
    private readonly IAccountService accountService;

    public ICommand SyncCommand
    {
        get => GetValue(() => SyncCommand); 
        set => SetValue(() => SyncCommand, value);
    }

    public ICommand LoginCommand
    {
        get => GetValue(() => LoginCommand);
        set => SetValue(() => LoginCommand, value);
    }

    public ICommand RefreshCommand
    {
        get => GetValue(() => RefreshCommand);
        set => SetValue(() => RefreshCommand, value);
    }

    public ICommand LogoutCommand
    {
        get => GetValue(() => LogoutCommand);
        set => SetValue(() => LogoutCommand, value);
    }

    public bool UserIsLoggedIn
    {
        get => GetValue(() => UserIsLoggedIn);
        set => SetValue(() => UserIsLoggedIn, value);
    }

    public UserAccountViewModel(ICommandFactory commandFactory, IFileSyncService fileSyncService, IAccountService accountService)
    {
        SyncCommand = ReactiveCommand.CreateFromTask(Sync);
        LoginCommand = ReactiveCommand.CreateFromTask(Login);
        LogoutCommand = ReactiveCommand.CreateFromTask(Logout);
        RefreshCommand = ReactiveCommand.CreateFromTask(Refresh);
        this.fileSyncService = fileSyncService;
        this.accountService = accountService;
    }

    public override string Header => "User Account";

    public async Task Sync()
    {
        await fileSyncService.Sync();
    }

    public async Task Login()
    {
        await accountService.Login();
        UserIsLoggedIn = true;
    }

    public async Task Refresh()
    {
        await fileSyncService.Get();
    }

    public async Task Logout()
    {
        await accountService.Logout();
        UserIsLoggedIn = false;
    }
}
