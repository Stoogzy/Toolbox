using MediatR;
using Toolbox.Application.Common.Interfaces;
using Toolbox.Application.Projects.Dtos;
using Toolbox.Core.Entities;

namespace Toolbox.Application.Projects.Commands.Create;

public class CreateProjectCommand : IRequest<ProjectDto>
{
    public required string CompanyName { get; init; }
    public required DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public string? Description { get; init; }
    public required decimal Budget {  get; init; }
    public List<Employee>? TeamMembers { get; init; }
}

internal class CreateProjectCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateProjectCommand, ProjectDto>
{
    public async Task<ProjectDto> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
    {
        Project project = new()
        {
            CompanyName = command.CompanyName,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            Description = command.Description,
            Budget = command.Budget,
            TeamMembers = command.TeamMembers ?? []
        };

        dbContext.Projects.Add(project);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ProjectDto
        {
            Id = project.Id,
            CompanyName = project.CompanyName,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Description = project.Description,
            Budget = project.Budget,
            TeamMembers = project.TeamMembers
        };
    }
}
