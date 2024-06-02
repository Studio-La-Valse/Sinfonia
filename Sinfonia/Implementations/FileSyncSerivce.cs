//using Sinfonia.Implementations.ScoreDocument.Converters;
//using Sinfonia.ViewModels.Application;
//using System.Net.Http;
//using System.Net.Http.Json;

//namespace Sinfonia.Implementations;
//internal class FileSyncSerivce : IFileSyncService
//{
//    private readonly DocumentCollectionViewModel documentCollectionViewModel;

//    public FileSyncSerivce(DocumentCollectionViewModel documentCollectionViewModel)
//    {
//        this.documentCollectionViewModel = documentCollectionViewModel;
//    }

//    public void UpdloadBorrowed()
//    {
//        var activeDocument = documentCollectionViewModel.TryGetActiveDocument(out var _activeDocument) ? _activeDocument : throw new Exception();
//        var _document = activeDocument.ScoreDocumentCore.GetMemento();
//        if(_document.Layout is null)
//        {
//            throw new Exception();
//        }
//        using var client = new HttpClient();

//        var content = JsonContent.Create(_document);
//        var uri = $"https://localhost:8081/api/document/{_document.Id}";
//        var response = AsyncHelper.RunSync(() => client.PutAsync(uri, content));

//        content = JsonContent.Create(_document);
//        uri = $"https://localhost:8081/api/layout/{_document.Layout.Id}";
//        response = AsyncHelper.RunSync(() => client.PutAsync(uri, content));
//    }

//    public void UploadPrivate()
//    {
//        var activeDocument = documentCollectionViewModel.TryGetActiveDocument(out var _activeDocument) ? _activeDocument : throw new Exception();
//        var _document = activeDocument.ScoreDocumentCore.GetMemento();
//        using var client = new HttpClient();

//        var content = JsonContent.Create(_document);
//        var uri = $"https://localhost:8081/api/document";
//        var response = AsyncHelper.RunSync(() => client.PutAsync(uri, content));
//    }
//}
