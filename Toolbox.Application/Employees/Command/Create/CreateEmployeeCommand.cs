using MediatR;
using Toolbox.Application.Common.Interfaces;
using Toolbox.Application.Employees.Dtos;
using Toolbox.Core.Entities;

namespace Toolbox.Application.Employees.Command.Create;

public class CreateEmployeeCommand : IRequest<EmployeeDto>
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required DateTime DateOfBirth { get; init; }
    public required string NationalInsuranceNumber { get; init; }
    public required decimal Salary { get; init; }
    public required DateTime StartDate {  get; init; }

}

internal class CreateEmployeeCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    public async Task<EmployeeDto> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        Employee employee = new()
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            DateOfBirth = command.DateOfBirth,
            NationalInsuranceNumber = command.NationalInsuranceNumber,
            Salary = command.Salary,
            StartDate = command.StartDate,
        };

        dbContext.Employees.Add(employee);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new EmployeeDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Age = employee.Age,
            NationalInsuranceNumber = employee.NationalInsuranceNumber,
            Salary = employee.Salary,
            StartDate = employee.StartDate,
            InProbation = employee.InProbation,
        };
    }
}
