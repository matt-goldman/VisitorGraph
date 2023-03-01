using GraphVisitor.Common.DTOs;
using GraphVisitor.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GraphVisitor.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class VisitController : ControllerBase
{
	public VisitController(IVisitService visitService)
	{
        _visitService = visitService;
    }

    public IVisitService _visitService { get; }

    [HttpPost("signin")]
    public async Task<ActionResult> SignIn(SignInDto signIn)
    {
        await _visitService.SignIn(signIn);

        return Ok();
    }

    [HttpPost("signout")]
    public async Task<ActionResult> SignOut(SignOutDto signOut)
    {
        await _visitService.SignOut(signOut);

        return Ok();
    }
}
