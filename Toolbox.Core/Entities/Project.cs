using Toolbox.Core.Common;

namespace Toolbox.Core.Entities;

public class Project : BaseEntity
{
    public required string CompanyName { get; set; }
    public required DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; } = null!;
    public string? Description { get; set; }
    public required decimal Budget { get; set; }

    // Navigation Properties.
    public List<Employee> TeamMembers { get; set; } = [];
    public List<Sprint> Sprints { get; set; } = [];
}
