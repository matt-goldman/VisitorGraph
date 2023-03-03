using GraphVisitor.Common.DTOs;
using GraphVisitor.WebApi.Data;
using GraphVisitor.WebApi.Exceptions;
using GraphVisitor.WebApi.Models;
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

    public async Task SignOut(SignOutDto signOut)
    {
        var signIn = await _dbContext.Visits
            .Where(v => v.Visitor.Email == signOut.VisitorEmail && v.SignedOut == null)
            .OrderByDescending(v => v.SignedIn)
            .ToListAsync();

        if (signIn is null || signIn.Count == 0)
        {
            throw new NotFoundException("No sign in found for this visitor");
        }

        if (signIn.Count > 1)
        {
            _logger.LogWarning($"Multiple sign ins found for this visitor {signOut.VisitorEmail}");
        }

        signIn.First().SignedOut = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
    }
}
