using GraphVisitor.Common.DTOs;
using GraphVisitor.WebApi.Data;

namespace GraphVisitor.WebApi.Services;

public interface IStaffService
{
    Task<IEnumerable<StaffDto>> SerachStaff(string searchTerm);
}

public class StaffService : IStaffService
{
    private readonly IGraphService _graphService;
    private readonly AppDbContext _appDbContext;
    private readonly ILogger<StaffService> _logger;

    public StaffService(IGraphService graphService, AppDbContext appDbContext, ILogger<StaffService> logger)
    {
        _graphService = graphService;
        _appDbContext = appDbContext;
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
