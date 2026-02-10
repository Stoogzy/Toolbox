using Microsoft.EntityFrameworkCore;
using Toolbox.Application.Common.Interfaces;
using Toolbox.Core.Entities;

namespace Toolbox.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    //DbSet Entities Here:
    public DbSet<Project> Projects { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Sprint> Sprints { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
