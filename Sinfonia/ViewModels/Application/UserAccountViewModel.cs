using Microsoft.CodeAnalysis;
using ReactiveUI;
using Sinfonia.Implementations;
using Sinfonia.ViewModels.Base;
using StudioLaValse.ScoreDocument.Models;
using System.Threading.Tasks;

namespace Sinfonia.ViewModels.Application;
public class UserAccountViewModel : SideBarContentViewModel
{
    private readonly IFileSyncService fileSyncService;
    private readonly IAccountService accountService;
    private readonly IDocumentViewModelFactory documentViewModelFactory;
    private readonly DocumentCollectionViewModel documentCollectionViewModel;

    public ObservableCollection<ScoreDocumentMetaDataModel> ScoreDocuments { get; } = [];

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

    public UserAccountViewModel(ICommandFactory commandFactory, IFileSyncService fileSyncService, IAccountService accountService, IDocumentViewModelFactory documentViewModelFactory, DocumentCollectionViewModel documentCollectionViewModel)
    {
        CreateCommand = ReactiveCommand.CreateFromTask(CreateNew);
        RegisterCommand = ReactiveCommand.CreateFromTask(accountService.Register); 

        SyncCommand = ReactiveCommand.CreateFromTask(Sync);
        LoginCommand = ReactiveCommand.CreateFromTask(Login);
        LogoutCommand = ReactiveCommand.CreateFromTask(Logout);
        RefreshCommand = ReactiveCommand.CreateFromTask(Refresh);

        this.fileSyncService = fileSyncService;
        this.accountService = accountService;
        this.documentViewModelFactory = documentViewModelFactory;
        this.documentCollectionViewModel = documentCollectionViewModel;
    }

    public override string Header => "User Account";


    public async Task Login()
    {
        try
        {
            var name = await accountService.Login();
            UserIsLoggedIn = true;
            HelloText = $"Hello, {name.Email}";
        }
        catch
        {

        }
        finally
        {

        }
    }


    public async Task Logout()
    {
        try
        {
            await accountService.Logout();
        }
        catch
        {

        }
        finally
        {
            UserIsLoggedIn = false;
        }
    }

    public async Task CreateNew()
    {
        try
        {
            await fileSyncService.CreateNew();
            await Refresh();    
        }
        catch
        {
            await Logout();
        }
        finally
        {

        }
    }

    public async Task Sync()
    {
        try
        {
            await fileSyncService.Sync();
        }
        catch 
        {
            await Logout();
        }
        finally
        {

        }
    }


    public async Task Refresh()
    {
        try
        {
            ScoreDocuments.Clear();

            var files = await fileSyncService.Get();
            foreach (var file in files)
            {
                ScoreDocuments.Add(file);
            }
        }
        catch
        {
            await Logout();
        }
        finally
        {

        }
    }

    public async Task Restore(ScoreDocumentMetaDataModel metaData)
    {
        ScoreDocumentModel? documentModel = null;

        try
        {
            documentModel = await fileSyncService.Get(metaData);
        }
        catch
        {
            await Logout();
            return;
        }
        finally
        {

        }

        var viewModel = documentViewModelFactory.Create(documentModel, metaData);
        documentCollectionViewModel.Add(viewModel);
    }

    public async Task Delete(ScoreDocumentMetaDataModel scoreDocumentMetaDataModel)
    {
        try
        {
            await fileSyncService.Delete(scoreDocumentMetaDataModel);
            await Refresh();
        }
        catch 
        {
            await Logout();
        }
        finally
        {

        }
    }
}
