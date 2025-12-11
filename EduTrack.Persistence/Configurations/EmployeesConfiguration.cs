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

            builder.Property(e => e.DepartmentId).HasColumnName("DepartmentId");
            builder.Property(e => e.PositionId).HasColumnName("PositionId");

            builder.Property(e => e.HireDate).IsRequired();
            builder.Property(e => e.IsActive).HasDefaultValue(true);
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("getdate()");

            builder.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Position)
                .WithMany(p => p.Employees)
                .HasForeignKey(e => e.PositionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
