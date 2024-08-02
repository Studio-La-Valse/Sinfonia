using StudioLaValse.ScoreDocument.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace Sinfonia.Interfaces;

public interface IRegisterService
{
    Task Register();
}

internal class RegisterService : IRegisterService
{
    public async Task Register()
    {
        using var client = new HttpClient();
        
        var email = "admin@admin.com";
        var password = "admin";
        var request = JsonContent.Create(new { email, password });
        var url = @"https://localhost:8081/register";
        var response = await client.PostAsync(url, request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
    }
}
