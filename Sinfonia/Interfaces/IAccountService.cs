using Avalonia.Controls;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Sinfonia.Interfaces;

public interface IAccountService
{
    bool UserIsLoggedIn { get; }
    string? AccessToken { get; }

    Task Login();
    bool ShouldRefresh();
    Task Refresh();
    Task Logout();
}


internal class AccountService : IAccountService
{
    private readonly AuthorizationCodeInstalledApp authCode;

    private UserCredential? userCredential;

    public AccountService(IConfiguration configuration)
    {
        var clientSecrets = new ClientSecrets()
        {
            ClientId = configuration.GetValue<string>("GoogleAuth:client_id"),
            ClientSecret = configuration.GetValue<string>("GoogleAuth:client_secret")
        };
        var scope = new string[] { "https://www.googleapis.com/auth/userinfo.email", "openid" };
        var codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            DataStore = new FileDataStore(AppDomain.CurrentDomain.BaseDirectory),
            Scopes = scope,
            ClientSecrets = clientSecrets
        });

        var codeReceiver = new LocalServerCodeReceiver();
        authCode = new AuthorizationCodeInstalledApp(codeFlow, codeReceiver);
    }

    public bool UserIsLoggedIn => userCredential != null;

    public string? AccessToken => userCredential?.Token?.IdToken;

    public async Task Login()
    {
        userCredential = await authCode.AuthorizeAsync("SinfoniaUser", CancellationToken.None);

        if (ShouldRefresh())
        {
            await Refresh();
        }
    }

    public async Task Refresh()
    {
        if (userCredential is null)
        {
            await Login();
            return;
        }

        await userCredential.RefreshTokenAsync(CancellationToken.None);
    }

    public async Task Logout()
    {
        if (userCredential is null)
        {
            throw new InvalidOperationException("User not loggin in.");
        }

        if (ShouldRefresh())
        {
            await Refresh();
        }

        await userCredential.RevokeTokenAsync(new CancellationToken());
        userCredential = null;
    }

    public bool ShouldRefresh()
    {
        if(userCredential is null)
        {
            throw new InvalidOperationException("User not loggin in.");
        }

        return authCode.ShouldRequestAuthorizationCode(userCredential.Token);
    }
}
