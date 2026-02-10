using MediatR;
using Microsoft.EntityFrameworkCore;
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
            throw new Exception($"Employee with ID: {command.Id}, not found.");
        }


    }
}
