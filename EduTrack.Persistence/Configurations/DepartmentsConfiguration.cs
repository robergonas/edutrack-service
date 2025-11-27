using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EduTrack.Domain.Entities;

namespace EduTrack.Persistence.Configurations{
    public class DepartmentsConfiguration: IEntityTypeConfiguration<Department>
    {   public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");
            builder.HasKey(d => d.DepartmentId);
            builder.Property(d => d.DepartmentName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(d => d.Description)
                .HasMaxLength(500);            
            builder.Property(d => d.CreatedAt)
                .IsRequired();
            builder.Property(d => d.CreatedBy)
                .HasMaxLength(100);
            builder.Property(d => d.ModifiedAt)
                .IsRequired();
            builder.Property(d => d.ModifiedBy)
                .HasMaxLength(100);
        }
    }
}
