using EduTrack.Domain.Entities;
using EduTrack.Domain.Models.Views;
using EduTrack.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace EduTrack.Persistence;

public class EduTrackDbContext : DbContext
{
    public EduTrackDbContext(DbContextOptions<EduTrackDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<Employees> Employees { get; set; }
    public DbSet<UserRoles> UserRoles { get; set; }
    public DbSet<Permissions> Persmissions { get; set; }
    public DbSet<UserEffectivePermission> UserEffectivePermissions { get; set; }
    public DbSet<Teacher> Teachers{ get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Position> Positions { get; set; }
    // Add other entities as needed (e.g., Employee, Department, Position)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserSessionConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeesConfiguration());  
        modelBuilder.ApplyConfiguration(new UserRolesConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionsConfiguration());
        modelBuilder.ApplyConfiguration(new RolePermissionsConfiguration());
        modelBuilder.ApplyConfiguration(new DepartmentsConfiguration());
        modelBuilder.ApplyConfiguration(new PositionsConfiguration());

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserEffectivePermission>()
        .HasNoKey()
        .ToView("vw_UserEffectivePermissions");

        // Optional: schema default
        //modelBuilder.HasDefaultSchema("dbo");
    }
}