using Microsoft.EntityFrameworkCore;
using Toolbox.Core.Entities;

namespace Toolbox.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    // Add DbSets here so the Application Layer can access them.
    DbSet<Project> Projects { get; set; }
    DbSet<Employee> Employees { get; set; }
    DbSet<Sprint> Sprints { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
