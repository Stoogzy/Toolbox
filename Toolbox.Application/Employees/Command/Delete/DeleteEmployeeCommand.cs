using MediatR;
using Microsoft.EntityFrameworkCore;
using Toolbox.Application.Common.Exceptions;
using Toolbox.Application.Common.Interfaces;
using Toolbox.Core.Entities;

namespace Toolbox.Application.Employees.Command.Delete;

public record DeleteEmployeeCommand(Guid Id) : IRequest;

internal class DeleteEmployeCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<DeleteEmployeeCommand>
{
    public async Task Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
    {
        Employee? employee = await dbContext.Employees
            .Where(e => e.Id == command.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (employee == null)
        {
            throw new NotFoundException(nameof(Employee), command.Id);
        }

        dbContext.Employees .Remove(employee);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}