using EduTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduTrack.Persistence.Configurations
{
    public class TeacherConfiguration: IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.ToTable("Teachers");
            builder.HasKey(x => x.TeacherId);

            builder.Property(x => x.Specialty).HasMaxLength(100);
            builder.Property(x => x.Degree).HasMaxLength(100);
            builder.Property(x => x.HireDate).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.CreatedAt).HasColumnType("datetime2(7)");
            builder.Property(x => x.UpdatedAt).HasColumnType("datetime2(7)");
            builder.Property(x => x.CreatedBy).HasMaxLength(100);
            builder.Property(x => x.UpdatedBy).HasMaxLength(100);

            builder.HasOne(x => x.Employee)
                .WithMany() 
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
