using Toolbox.Core.Common;

namespace Toolbox.Core.Entities;

public class Sprint : BaseEntity
{
    public required string Title { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    // public required List<Task> Tasks { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
}
