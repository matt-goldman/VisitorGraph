using CommunityToolkit.Maui.Views;
using GraphVisitor.Common.DTOs;
using GraphVisitor.UI.Popups;
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
        try
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
        catch (Exception)
        {
            var popup = new Dialog("Error", "Service not currently available");
            var closed = await App.Current.MainPage.ShowPopupAsync(popup);
            return Enumerable.Empty<StaffDto>();
        }
    }
}
