using GraphVisitor.Common.DTOs;
using GraphVisitor.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GraphVisitor.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StaffController : ControllerBase
{
	public StaffController(IStaffService staffService)
	{
        _staffService = staffService;
    }

    public IStaffService _staffService { get; }

    [HttpGet("search/{searchTerm}")]
    public async Task<IEnumerable<StaffDto>> Search(string searchTerm)
    {
        return await _staffService.SerachStaff(searchTerm);
    }
}
