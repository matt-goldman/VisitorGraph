using GraphVisitor.Common.DTOs;

namespace GraphVisitor.WebApi.Services;

public interface IStaffService
{
    Task<IEnumerable<StaffDto>> SerachStaff(string searchTerm);
}

public class StaffService : IStaffService
{
    private readonly IGraphService _graphService;
    private readonly ILogger<StaffService> _logger;

    public StaffService(IGraphService graphService, ILogger<StaffService> logger)
    {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<IEnumerable<StaffDto>> SerachStaff(string searchTerm)
    {
        var results = new List<StaffDto>();
        var graphResults = await _graphService.SearchStaff(searchTerm);

        foreach (var result in graphResults)
        {
            results.Add(new StaffDto
            {
                Department  = result.Department,
                DisplayName = $"{result.FirstName} {result.LastName}",
                StaffId     = result.GraphId
            });
        }

        return results;
    }
}
