using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Toolbox.Core.Entities;

namespace Toolbox.Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.CompanyName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.StartDate)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(200);

        builder.Property(p => p.Budget)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasMany(p => p.TeamMembers)
            .WithMany(e => e.Projects)
            .UsingEntity(j => j.ToTable("ProjectMembers"));
    }
}
