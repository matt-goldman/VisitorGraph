using GraphVisitor.Common.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace GraphVisitor.UI.Services;

public interface IVisitService
{
    Task<bool> SignIn(SignInDto dto);
    Task<SignoutStatus> SignOut(SignOutDto dto);
}

public enum SignoutStatus
{
    Success,
    Error,
    NotFound
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

    public async Task<SignoutStatus> SignOut(SignOutDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/visit/signout", dto);
            if (response.IsSuccessStatusCode)
            {
                return SignoutStatus.Success;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return SignoutStatus.NotFound;
            }
            else
            {
                return SignoutStatus.Error;
            }
        }
        catch (Exception)
        {
            return SignoutStatus.Error;
        }
    }
}
