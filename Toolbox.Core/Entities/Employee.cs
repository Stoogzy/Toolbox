using Toolbox.Core.Common;

namespace Toolbox.Core.Entities;

public class Employee : BaseEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }    
    public required DateTime DateOfBirth { get; set; }
    public required string NationalInsuranceNumber { get; set; }
    public required decimal Salary { get; set; }
    public required DateTime StartDate {  get; set; }

    public string FullName => $"{FirstName} {LastName}";
    public int Age => DateTime.UtcNow.Year - DateOfBirth.Year - (DateTime.UtcNow.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
    public bool InProbation => DateTime.UtcNow < StartDate.AddMonths(3);

    // Navigation Properties.
    public List<Project> Projects { get; set; } = [];
}
