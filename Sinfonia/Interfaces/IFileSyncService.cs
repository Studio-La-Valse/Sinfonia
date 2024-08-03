using Sinfonia.ViewModels.Application;
using StudioLaValse.ScoreDocument.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net;
using StudioLaValse.ScoreDocument.Models.EndToEnd;

namespace Sinfonia.Interfaces;

public interface IFileSyncService
{
    Task CreateNew();
    Task<IEnumerable<ScoreDocumentMetaDataModel>> Get();
    Task<ScoreDocumentModel> Get(ScoreDocumentMetaDataModel model);
    Task Sync();
    Task Delete(ScoreDocumentMetaDataModel model);
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

        var httpHandler = new HttpClientHandler() { CookieContainer = cookieContainer, UseCookies = true };
        using var client = new HttpClient(httpHandler);
        var scoreDocument = activeDocument.ScoreDocument.Freeze();
        var requestBody = new CreateScoreDocumentRequest()
        {
            MetaData = activeDocument.MetaData,
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

    public async Task<ScoreDocumentModel> Get(ScoreDocumentMetaDataModel model)
    {
        var httpHandler = new HttpClientHandler() { CookieContainer = cookieContainer, UseCookies = true };
        using var client = new HttpClient(httpHandler);
        var response = await client.GetAsync($@"https://localhost:8081/api/scoredocument/{model.ScoreDocumentId}");
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadFromJsonAsync<ScoreDocumentResponse>() ?? throw new Exception("Invalid reponse.");
        return responseContent.ScoreDocument;
    }

    public async Task Sync()
    {
        throw new NotImplementedException();
    }

    public async Task Delete(ScoreDocumentMetaDataModel model)
    {
        var uri = new Uri($@"https://localhost:8081/api/scoredocument/{model.ScoreDocumentId}");
        var httpHandler = new HttpClientHandler() { CookieContainer = cookieContainer, UseCookies = true };
        using var client = new HttpClient(httpHandler);
        var response = await client.DeleteAsync(uri);
        response.EnsureSuccessStatusCode();
    }
}
