using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sinfonia.Interfaces;

public interface IAccountService
{
    bool UserIsLoggedIn { get; }

    Task Login();
    Task Logout();
    Task<UserNameResponse> Name();
}


internal class AccountService : IAccountService
{
    private readonly CookieContainer cookieContainer;

    public AccountService(CookieContainer cookieContainer)
    {
        this.cookieContainer = cookieContainer;
    }

    public bool UserIsLoggedIn => cookieContainer.Count > 0;


    public async Task Login()
    {
        var httpHandler = new HttpClientHandler() { CookieContainer = cookieContainer, UseCookies = true };
        var url = new Uri(@"https://localhost:8081/login?useCookies=true&useSessionCookies=true");
        using var client = new HttpClient(httpHandler);
        var email = "admin@admin.com";
        var password = "admin";
        var request = JsonContent.Create(new { email, password });
       
        var response = await client.PostAsync(url, request);
        response.EnsureSuccessStatusCode();

        await WriteCookies();
    }

    public async Task WriteCookies()
    {
        try
        {
            var directory = Directory.GetCurrentDirectory();
            var path = Path.Combine(directory, "cookies.json");
            await using var fs = File.Create(path);
            JsonSerializer.Serialize(fs, cookieContainer.GetAllCookies());
        }
        catch 
        { 
        
        }
        finally
        {

        }
    }

    public async Task RestoreCookies()
    {
        try
        {
            var directory = Directory.GetCurrentDirectory();
            var path = Path.Combine(directory, "cookies.json");
            await using var fs = File.OpenRead(path);
            var cookieCollection = JsonSerializer.Deserialize<CookieCollection>(fs)!;
            cookieContainer.Add(cookieCollection);
        }
        catch 
        { 
        
        }
        finally
        {

        }
    }

    public void ExpireAllCookies()
    {
        foreach (var cookie in cookieContainer.GetAllCookies().OfType<Cookie>())
        {
            cookie.Expired = true;
        }
    }

    public async Task Logout()
    {
        var httpHandler = new HttpClientHandler() { CookieContainer = cookieContainer, UseCookies = true };
        var url = new Uri(@"https://localhost:8081/logout");
        using var client = new HttpClient(httpHandler);
        var request = JsonContent.Create(new {});
        var response = await client.PostAsync(url, request);
        response.EnsureSuccessStatusCode();

        ExpireAllCookies();
        await WriteCookies();
    }

    public async Task<UserNameResponse> Name()
    {
        var uri = new Uri(@"https://localhost:8081/me");
        var httpHandler = new HttpClientHandler() { CookieContainer = cookieContainer, UseCookies = true };
        using var client = new HttpClient(httpHandler);
        var response = await client.GetAsync(uri);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<UserNameResponse>()
            ?? throw new Exception("Invalid response.");
        return content;
    }
}

public record UserNameResponse(string Email, Guid Id);
