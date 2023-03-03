using GraphVisitor.Common.DTOs;
using System.Net.Http.Json;

namespace GraphVisitor.UI.Services;

public interface IVisitService
{
    Task<bool> SignIn(SignInDto dto);
    Task<bool> SignOut(SignOutDto dto);
}

public class VisitService : IVisitService
{
    private readonly HttpClient _httpClient;
    
    public VisitService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(Constants.ApiClientName);
    }

    public async Task<bool> SignIn(SignInDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/visit/signin", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> SignOut(SignOutDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/visit/signout", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
}
