using GraphVisitor.WebApi.Models;

namespace GraphVisitor.WebApi.Services;

public interface IGraphService
{
    public IEnumerable<Staff> SearchStaff(string query);

    public Task SendNotification();
}
