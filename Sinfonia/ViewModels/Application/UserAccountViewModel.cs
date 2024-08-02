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

    public ICommand CreateCommand
    {
        get => GetValue(() => CreateCommand);
        set => SetValue(() => CreateCommand, value);
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

    public ICommand RegisterCommand
    {
        get => GetValue(() => RegisterCommand);
        set => SetValue(() => RegisterCommand, value);
    }

    public bool UserIsLoggedIn
    {
        get => GetValue(() => UserIsLoggedIn);
        set => SetValue(() => UserIsLoggedIn, value);
    }

    public string HelloText
    {
        get => GetValue(() => HelloText);
        set => SetValue(() => HelloText, value);
    }

    public UserAccountViewModel(ICommandFactory commandFactory, IFileSyncService fileSyncService, IAccountService accountService, IRegisterService registerService)
    {
        CreateCommand = ReactiveCommand.CreateFromTask(fileSyncService.CreateNew);
        SyncCommand = ReactiveCommand.CreateFromTask(Sync);
        LoginCommand = ReactiveCommand.CreateFromTask(Login);
        LogoutCommand = ReactiveCommand.CreateFromTask(Logout);
        RefreshCommand = ReactiveCommand.CreateFromTask(Refresh);
        RegisterCommand = ReactiveCommand.CreateFromTask(registerService.Register);
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

        var name = await accountService.Name();
        HelloText = $"Hello, {name.Email}";
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
