using GraphVisitor.WebApi.Models;

namespace GraphVisitor.WebApi.Services;

public interface IGraphService
{
    public Task<IEnumerable<Staff>> SearchStaff(string query);

    public Task SendNotification(string staffId, string visitorName, string visitorEmail);
}
