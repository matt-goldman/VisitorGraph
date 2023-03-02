using GraphVisitor.Common.DTOs;
using GraphVisitor.WebApi.Data;
using GraphVisitor.WebApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace GraphVisitor.WebApi.Services;

public interface IVisitService
{
    Task SignIn(SignInDto signIn);

    Task SignOut(SignOutDto signOut);
}

public class VisitService : IVisitService
{
    private readonly IGraphService _graphService;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<VisitService> _logger;

    public VisitService(IGraphService graphService, AppDbContext dbContext, ILogger<VisitService> logger)
    {
        _graphService = graphService;
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task SignIn(SignInDto signIn)
    {
        await _graphService.SendNotification(signIn.StaffId, signIn.VisitorName, signIn.VisitorEmail);

        try
        {
            var visitor = await _dbContext.Visitors.FirstOrDefaultAsync(v => v.Email == signIn.VisitorEmail);

            if (visitor is null)
            {
                visitor = new Visitor
                {
                    Email = signIn.VisitorEmail,
                    Name = signIn.VisitorName
                };
            }

            var staff = await _dbContext.Staff.FirstOrDefaultAsync(s => s.GraphId == signIn.StaffId);

            if (staff is null)
            {
                staff = new Staff
                {
                    GraphId = signIn.StaffId
                };
            }

            var visit = new Visit
            {
                Visitor = visitor,
                StaffMember = staff,
                SignedIn = DateTime.UtcNow
            };

            _dbContext.Visits.Add(visit);

            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error recording visit", ex);
            throw;
        }
    }

    public Task SignOut(SignOutDto signOut)
    {
        throw new NotImplementedException();
    }
}
