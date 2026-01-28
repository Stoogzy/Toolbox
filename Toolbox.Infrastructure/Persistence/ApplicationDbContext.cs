using Microsoft.EntityFrameworkCore;
using Toolbox.Application.Common.Interfaces;
using Toolbox.Core.Entities;

namespace Toolbox.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    //DbSet Entities Here:
    DbSet<Project> Projects { get; set; }
    DbSet<Employee> Employees { get; set; }
    DbSet<Sprint> Sprints { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
