using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Toolbox.Core.Entities;

namespace Toolbox.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser(
    ILogger<ApplicationDbContextInitialiser> logger,
    ApplicationDbContext dbContext)
{
    public async Task InitialiseAsync()
    {
        try
        {
            // Apply any pending migrations automatically.
            if (dbContext.Database.IsSqlServer())
            {
                await dbContext.Database.MigrateAsync();
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        // Only seed the database if it's empty.
        if (!dbContext.Employees.Any())
        {
            Employee techLead = new()
            {
                FirstName = "Tech",
                LastName = "Lead",
                DateOfBirth = new DateTime(1990, 07, 06),
                NationalInsuranceNumber = "AB123456C",
                Salary = 80000,
                StartDate = DateTime.UtcNow.AddYears(-5),
            };

            Employee netDev = new()
            {
                FirstName = ".Net",
                LastName = "Developer",
                DateOfBirth = new DateTime(1995, 03, 07),
                NationalInsuranceNumber = "DE123456F",
                Salary = 50000,
                StartDate = DateTime.UtcNow.AddYears(-2)
            };

            Employee juniorDev = new()
            {
                FirstName = "Junior",
                LastName = "Developer",
                DateOfBirth = new DateTime(1995, 04, 24),
                NationalInsuranceNumber = "GH123456I",
                Salary = 30000,
                StartDate = DateTime.UtcNow
            };

            Project project1 = new()
            {
                CompanyName = "Capsule Corp",
                Description = "Design and provide new website to showcase latest capsule tech.",
                Budget = 200000,
                StartDate = DateTime.UtcNow.AddDays(7),
                TeamMembers = [techLead, juniorDev],
                Sprints = 
                [
                    new Sprint
                    {
                        Title = "Scaffold Codebase",
                        StartDate = DateTime.UtcNow.AddDays(7),
                        EndDate = DateTime.UtcNow.AddDays(21)
                    }
                ]
            };

            Project project2 = new()
            {
                CompanyName = "Kame Dojo",
                Description = "Design and provide new website for Kame Dojo martial arts classes.",
                Budget = 80000,
                StartDate = DateTime.UtcNow.AddDays(14),
                TeamMembers = [netDev],
                Sprints =
                [
                    new Sprint
                    {
                        Title = "Scaffold Codebase",
                        StartDate = DateTime.UtcNow.AddDays(14),
                        EndDate = DateTime.UtcNow.AddDays(28)
                    }
                ]
            };

            List<Project> projects = [project1, project2];

            // EF Core will automatically add all entities as they are associated with the projects.
            dbContext.Projects.AddRange(projects);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            logger.LogInformation("Database seeded successfully.");
        }
    }
}
