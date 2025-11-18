using EduTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace EduTrack.Persistence.Configurations
{
    public class RolePermissionsConfiguration: IEntityTypeConfiguration<RolePermissions>
    {
        public void Configure(EntityTypeBuilder<RolePermissions> builder)
        {
            builder.ToTable("RolePermissions");
            builder.HasKey(rp => rp.idRolePermission);
            builder.Property(rp => rp.RoleId)
                .IsRequired();
            builder.Property(rp => rp.PermissionId)
                .IsRequired();
        }
    }
}
