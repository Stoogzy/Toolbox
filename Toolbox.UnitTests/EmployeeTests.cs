using Toolbox.Core.Entities;

namespace Toolbox.UnitTests;

public class EmployeeTests
{
    private Employee CreateEmployee(
        string firstName = "Dominik",
        string lastName = "Mysterio",
        DateTime? dateOfBirth = null,
        DateTime? startDate = null,
        decimal salary = 50000m) => new()
        {
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = dateOfBirth ?? new DateTime(1990, 1, 1),
            NationalInsuranceNumber = "AA 12 34 56 A",
            Salary = salary,
            StartDate = startDate ?? DateTime.UtcNow.AddMonths(-6)
        };

    [Fact]
    public void FullName_CombinesFirstAndLastName()
    {
        Employee employee = CreateEmployee("Dominik", "Mysterio");

        Assert.Equal("Dominik Mysterio", employee.FullName);
    }

    [Fact]
    public void FullName_HandlesHyphenatedLastName()
    {
        Employee employee = CreateEmployee("Jane", "Smith-Jones");

        Assert.Equal("Jane Smith-Jones", employee.FullName);
    }

    [Fact]
    public void Age_IsCalculatedCorrectly()
    {
        // Born exactly 30 years ago today.
        DateTime dob = new DateTime(DateTime.UtcNow.Year - 30, 1, 1);
        Employee employee = CreateEmployee(dateOfBirth: dob);

        Assert.Equal(30, employee.Age);
    }

    [Fact]
    public void Age_BeforeBirthdayThisYear_IsOneYearLess()
    {
        // Born tomorrow's date but 30 years ago, birthday hasn't passed yet.
        DateTime dob = new DateTime(DateTime.UtcNow.Year - 30, 12, 31);
        Employee employee = CreateEmployee(dateOfBirth: dob);

        Assert.Equal(29, employee.Age);
    }

    [Fact]
    public void InProbation_WhenStartedOneMonthAgo_IsTrue()
    {
        Employee employee = CreateEmployee(startDate: DateTime.UtcNow.AddMonths(-1));

        Assert.True(employee.InProbation);
    }

    [Fact]
    public void InProbation_WhenStartedSixMonthsAgo_IsFalse()
    {
        Employee employee = CreateEmployee(startDate: DateTime.UtcNow.AddMonths(-6));

        Assert.False(employee.InProbation);
    }
}
