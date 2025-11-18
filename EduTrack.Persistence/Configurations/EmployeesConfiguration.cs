using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EduTrack.Domain.Entities;

namespace EduTrack.Persistence.Configurations{
    public class EmployeesConfiguration: IEntityTypeConfiguration<Employees>
    {
        public void Configure(EntityTypeBuilder<Employees> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(e => e.EmployeeId);

            builder.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.DepartmentID)
                .IsRequired();

            builder.Property(e => e.PositionID)
                .IsRequired();

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}
