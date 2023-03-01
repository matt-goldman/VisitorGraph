using GraphVisitor.Common.DTOs;

namespace GraphVisitor.WebApi.Services;

public interface IStaffService
{
    Task<IEnumerable<StaffDto>> SerachStaff(string searchTerm);
}

public class StaffService : IStaffService
{
    public Task<IEnumerable<StaffDto>> SerachStaff(string searchTerm)
    {
        throw new NotImplementedException();
    }
}
