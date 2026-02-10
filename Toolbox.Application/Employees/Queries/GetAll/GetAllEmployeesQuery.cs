using MediatR;
using Microsoft.EntityFrameworkCore;
using Toolbox.Application.Common.Interfaces;
using Toolbox.Application.Employees.Dtos;

namespace Toolbox.Application.Employees.Queries.GetAll;

public record GetAllEmployeesQuery : IRequest<List<EmployeeDto>>
{
}

internal class GetAllEmployeesQueryHandler(IApplicationDbContext dbContext) 
    : IRequestHandler<GetAllEmployeesQuery, List<EmployeeDto>>
{
    public async Task<List<EmployeeDto>> Handle(GetAllEmployeesQuery query, CancellationToken cancellationToken)
    {
        return await dbContext.Employees
            .AsNoTracking() // Doing this as we are performing a read-only action.
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                FullName = e.FullName,
                Age = e.Age,
                NationalInsuranceNumber = e.NationalInsuranceNumber,
                Salary = e.Salary,
                StartDate = e.StartDate,
                InProbation = e.InProbation,
            })
            .ToListAsync(cancellationToken);
    }
}