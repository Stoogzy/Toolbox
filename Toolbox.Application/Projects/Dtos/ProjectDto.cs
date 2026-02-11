using Toolbox.Core.Entities;

namespace Toolbox.Application.Projects.Dtos;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public decimal Budget { get; set; }
    public List<Employee>? TeamMembers { get; set; }
    public List<Sprint>? Sprints { get; set; }
}
