namespace Toolbox.Application.Employees.Dtos;

public class EmployeeDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string? NationalInsuranceNumber { get; set; }
    public decimal Salary { get; set; }
    public DateTime StartDate { get; set; }
    public bool InProbation { get; set; }
}
