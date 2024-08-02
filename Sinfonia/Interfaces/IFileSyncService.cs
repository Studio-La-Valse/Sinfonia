using Sinfonia.ViewModels.Application;
using StudioLaValse.ScoreDocument.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net;
using System.Runtime.InteropServices;
using StudioLaValse.ScoreDocument.Models.EndToEnd;

namespace Sinfonia.Interfaces;

public interface IFileSyncService
{
    Task CreateNew();
    Task<IEnumerable<ScoreDocumentMetaDataModel>> Get();
    Task Sync();
}

internal class FileSyncService : IFileSyncService
{
    private readonly DocumentCollectionViewModel documentCollectionViewModel;
    private readonly CookieContainer cookieContainer;
    private readonly IAccountService accountService;

    public FileSyncService(DocumentCollectionViewModel documentCollectionViewModel, CookieContainer cookieContainer, IAccountService accountService)
    {
        this.documentCollectionViewModel = documentCollectionViewModel;
        this.cookieContainer = cookieContainer;
        this.accountService = accountService;
    }

    public async Task CreateNew()
    {
        var isActive = documentCollectionViewModel.TryGetActiveDocument(out var activeDocument);
        if (!isActive)
        {
            return;
        }

        var me = await accountService.Name();

        var httpHandler = new HttpClientHandler() { CookieContainer = cookieContainer, UseCookies = true };
        using var client = new HttpClient(httpHandler);
        var scoreDocument = activeDocument!.ScoreDocument.Freeze();
        var metaData = new ScoreDocumentMetaDataModel()
        {
            ScoreDocumentId = scoreDocument.Id,
            IsPublic = false,
            CreationDate = DateTime.Now,
            ComposerFullName = me.Email,
            CompositionMonth = DateTime.Now.Month,
            CompositionYear = DateTime.Now.Year,
            CompositionYearStart = DateTime.Now.Year,
            LastEditDate = DateTime.Now,
            Subtitle = "Hello, ",
            Title = "mom!",
        };
        var requestBody = new CreateScoreDocumentRequest()
        {
            MetaData = metaData,
            ScoreDocument = scoreDocument
        };
        var content = JsonContent.Create(requestBody);
        var response = await client.PostAsync(@"https://localhost:8081/api/scoredocument", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<ScoreDocumentMetaDataModel>> Get()
    {
        var httpHandler = new HttpClientHandler() { CookieContainer = cookieContainer, UseCookies = true };
        using var client = new HttpClient(httpHandler);
        var response = await client.GetAsync(@"https://localhost:8081/api/scoredocument");
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadFromJsonAsync<IEnumerable<ScoreDocumentMetaDataModel>>() ?? throw new Exception("Invalid reponse.");
        return responseContent;
    }

    public async Task Sync()
    {
        throw new NotImplementedException();
    }
}
