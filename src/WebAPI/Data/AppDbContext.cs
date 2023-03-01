using GraphVisitor.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphVisitor.WebApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Visitor> Visitors => Set<Visitor>();
    public DbSet<Visit> Visits => Set<Visit>();
}
