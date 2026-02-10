using MediatR;
using Microsoft.EntityFrameworkCore;
using Toolbox.Application.Common.Exceptions;
using Toolbox.Application.Common.Interfaces;
using Toolbox.Core.Entities;

namespace Toolbox.Application.Employees.Command.Update;

public record UpdateEmployeeCommand : IRequest
{
    public required Guid Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public decimal? Salary { get; init; }
}

internal class UpdateEmployeeCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpdateEmployeeCommand>
{
    public async Task Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        Employee? employee = await dbContext.Employees
            .Where(e => e.Id == command.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (employee == null)
        {
            throw new NotFoundException(nameof(Employee), command.Id);
        }

        if (command.FirstName != null) employee.FirstName = command.FirstName;
        if (command.LastName != null) employee.LastName = command.LastName;
        if (command.Salary != null) employee.Salary = command.Salary.Value;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
