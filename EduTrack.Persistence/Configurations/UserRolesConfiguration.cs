using EduTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduTrack.Persistence.Configurations
{
    public class UserRolesConfiguration: IEntityTypeConfiguration<UserRoles>
    {
        public void Configure(EntityTypeBuilder<UserRoles> builder)
        {
            builder.ToTable("UserRoles");

            builder.HasKey(ur => ur.UserRoleId);

            builder.Property(ur => ur.UserId)
                .IsRequired();

            builder.Property(ur => ur.RoleId)
                .IsRequired();
        }
    }
}
