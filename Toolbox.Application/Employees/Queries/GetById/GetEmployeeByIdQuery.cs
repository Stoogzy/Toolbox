using MediatR;
using Microsoft.EntityFrameworkCore;
using Toolbox.Application.Common.Exceptions;
using Toolbox.Application.Common.Interfaces;
using Toolbox.Application.Employees.Dtos;
using Toolbox.Core.Entities;

namespace Toolbox.Application.Employees.Queries.GetById;

public record GetEmployeeByIdQuery(Guid EmployeeId) : IRequest<EmployeeDto>;

internal class GetEmployeeByIdQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
{
    public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
    {
        EmployeeDto? employee = await dbContext.Employees
            .AsNoTracking()
            .Where(e => e.Id == query.EmployeeId)
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
            .FirstOrDefaultAsync(cancellationToken);

        return employee ?? throw new NotFoundException(nameof(Employee), query.EmployeeId);
    }
}
