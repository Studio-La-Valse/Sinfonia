using Sinfonia.ViewModels.Application;
using StudioLaValse.ScoreDocument.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace Sinfonia.Interfaces;

public interface IFileSyncService
{
    Task<IEnumerable<ScoreDocumentMetaDataModel>> Get();
    Task Sync();
}

internal class FileSyncService : IFileSyncService
{
    private readonly DocumentCollectionViewModel documentCollectionViewModel;
    private readonly IAccountService accountService;

    public FileSyncService(DocumentCollectionViewModel documentCollectionViewModel, IAccountService accountService)
    {
        this.documentCollectionViewModel = documentCollectionViewModel;
        this.accountService = accountService;
    }

    public async Task<IEnumerable<ScoreDocumentMetaDataModel>> Get()
    {
        if(accountService.AccessToken is null)
        {
            return [];
        }

        if (accountService.ShouldRefresh())
        {
            await accountService.Refresh();
        }

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accountService.AccessToken);
        var url = @"https://localhost:8081/api/scoredocument";
        var response = await client.GetAsync(url);
        var content = await response.Content.ReadFromJsonAsync<IEnumerable<ScoreDocumentMetaDataModel>>() ?? throw new Exception();
        return content;
    }

    public async Task Sync()
    {
        if(!documentCollectionViewModel.TryGetActiveDocument(out var activeDocument))
        {
            return;
        }

        var scoreDocument = activeDocument.ScoreBuilder.Freeze();
        var request = new StudioLaValse.ScoreDocument.Models.EndToEnd.CreateScoreDocumentRequest()
        {
            MetaData = new ScoreDocumentMetaDataModel()
            {
                Id = scoreDocument.Id,
                IsPublic = false,
                OwnerEmail = "roelwestrik@gmail.com",
                ScoreDocumentId = scoreDocument.Id,
            },
            ScoreDocument = scoreDocument
        };

        if (accountService.ShouldRefresh())
        {
            await accountService.Refresh();
        }

        using var client = new HttpClient();
        var url = @"https://localhost:8081/api/scoredocument";
        var content = JsonContent.Create(request);
        var response = await client.PostAsync(url, content);
    }
}
