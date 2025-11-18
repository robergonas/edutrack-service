using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EduTrack.Domain.Entities;

namespace EduTrack.Persistence.Configurations {
    public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
    {
        public void Configure(EntityTypeBuilder<UserSession> builder)
        {
            builder.ToTable("UserSessions");

            builder.HasKey(s => s.SessionId);

            builder.Property(s => s.RefreshToken)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(s => s.DeviceInfo)
                .HasMaxLength(255);

            builder.Property(s => s.IsActive)
                .HasDefaultValue(true);

            builder.Property(s => s.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}