using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Toolbox.Core.Entities;

namespace Toolbox.Infrastructure.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(50);

        // HasIndex
        // Creates a non-clustered index on the LastName column.
        // Benefit: Significantly speeds up searches (e.g., searching for employees by surname) 
        // and sorting operations at the cost of a small amount of disk space and write overhead.
        builder.HasIndex(e => e.LastName);

        builder.Property(e => e.DateOfBirth)
            .IsRequired();

        // National Insurance Number length validation.
        builder.Property(e => e.NationalInsuranceNumber)
            .IsRequired()
            .HasMaxLength(9)
            .IsFixedLength();

        // This prevents duplicate NI numbers at the database level
        builder.HasIndex(e => e.NationalInsuranceNumber)
            .IsUnique();

        // HasPrecision(18, 2)
        // 18 = Precision: The total maximum number of digits stored.
        // 2  = Scale: The number of digits to the right of the decimal point.
        // Benefit: Ensures financial data is stored accurately and consistently across different SQL environments.
        builder.Property(e => e.Salary)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(e => e.StartDate)
            .IsRequired();
    }
}
