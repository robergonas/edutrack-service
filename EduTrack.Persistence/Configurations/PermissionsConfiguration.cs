using EduTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduTrack.Persistence.Configurations
{
    public class PermissionsConfiguration: IEntityTypeConfiguration<Permissions>
    {
        public void Configure(EntityTypeBuilder<Permissions> builder)
        {
            builder.ToTable("Permissions");

            builder.HasKey(p => p.Permissionid);

            builder.Property(p => p.Module)
                .IsRequired();  

            builder.Property(p => p.Action)
                .IsRequired();

            builder.Property(p => p.Description)
                .IsRequired();
        }
    }
}
