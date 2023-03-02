using GraphVisitor.Common.DTOs;
using System.Net.Http.Json;

namespace GraphVisitor.UI.Services;

public interface IStaffService
{
    Task<IEnumerable<StaffDto>> Search(string searchTerm);
}

public class StaffService : IStaffService
{
    private readonly HttpClient _httpClient;
    
    public StaffService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(Constants.ApiClientName);
    }

    public async Task<IEnumerable<StaffDto>> Search(string searchTerm)
    {
        var response = await _httpClient.GetAsync($"/staff/search/{searchTerm}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<StaffDto>>();
        }
        else
        {
            return Enumerable.Empty<StaffDto>();
        }
    }
}
