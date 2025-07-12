using LexiFlow.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<RolePermission> RolePermissions { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<UserGroup> UserGroups { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationships
            modelBuilder.Entity<UserGroup>()
                .HasKey(ug => new { ug.UserId, ug.GroupId });

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.UserGroups)
                .HasForeignKey(ug => ug.UserId);

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.Group)
                .WithMany(g => g.UserGroups)
                .HasForeignKey(ug => ug.GroupId);

            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin", Description = "Administrator with full access" },
                new Role { Id = 2, Name = "User", Description = "Standard user with limited access" }
            );

            // Seed permissions
            modelBuilder.Entity<Permission>().HasData(
                new Permission { Id = 1, Name = "User.View", Description = "Can view user information" },
                new Permission { Id = 2, Name = "User.Create", Description = "Can create users" },
                new Permission { Id = 3, Name = "User.Edit", Description = "Can edit users" },
                new Permission { Id = 4, Name = "User.Delete", Description = "Can delete users" },
                new Permission { Id = 5, Name = "Content.Manage", Description = "Can manage content" }
            );

            // Assign permissions to roles
            modelBuilder.Entity<RolePermission>().HasData(
                // Admin has all permissions
                new RolePermission { RoleId = 1, PermissionId = 1 },
                new RolePermission { RoleId = 1, PermissionId = 2 },
                new RolePermission { RoleId = 1, PermissionId = 3 },
                new RolePermission { RoleId = 1, PermissionId = 4 },
                new RolePermission { RoleId = 1, PermissionId = 5 },
                // User has only view permission
                new RolePermission { RoleId = 2, PermissionId = 1 }
            );

            // Seed admin user (password: Admin@123)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    // Using BCrypt for password hashing (Admin@123)
                    PasswordHash = "$2a$11$kzQ3QXfqVtUlvXcMDaXG1OtIzfHLxlwMVdFLaqVUWJHXMfmdHX51W",
                    Email = "admin@lexiflow.com",
                    FullName = "System Administrator",
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    RoleId = 1
                }
            );
        }
    }
}
