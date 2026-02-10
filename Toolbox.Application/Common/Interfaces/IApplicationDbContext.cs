using Microsoft.EntityFrameworkCore;
using Toolbox.Core.Entities;

namespace Toolbox.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    // Add DbSets here so the Application Layer can access them.
    public DbSet<Project> Projects { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Sprint> Sprints { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
