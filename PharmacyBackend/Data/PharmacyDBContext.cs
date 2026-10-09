using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PharmacyBackend.Models;

namespace PharmacyBackend.Data
{
    public class PharmacyDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public PharmacyDbContext(DbContextOptions<PharmacyDbContext> options) : base(options) { }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One-to-One: Supplier-Drug
            modelBuilder.Entity<Supplier>()
                .HasOne(s => s.Drug)
                .WithOne(d => d.Supplier)
                .HasForeignKey<Drug>(d => d.SupplierId);

            
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Role Seeding
            var adminRoleId = Guid.NewGuid().ToString();
            var doctorRoleId = Guid.NewGuid().ToString();
            var supplierRoleId = Guid.NewGuid().ToString();

            modelBuilder.Entity<ApplicationRole>().HasData(
                new ApplicationRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
                new ApplicationRole { Id = doctorRoleId, Name = "Doctor", NormalizedName = "DOCTOR" },
                new ApplicationRole { Id = supplierRoleId, Name = "Supplier", NormalizedName = "SUPPLIER" }
            );

            // Admin User Seeding
            var adminUserId = Guid.NewGuid().ToString();
            var hasher = new PasswordHasher<ApplicationUser>();
            var adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin@pharmacy.com",
                Email = "admin@pharmacy.com",
                FullName = "Admin",
                Contact = "8542877593",
                NormalizedEmail = "ADMIN@PHARMACY.COM",
                NormalizedUserName = "ADMIN@PHARMACY.COM",
                PasswordHash = hasher.HashPassword(null, "Admin@123")
            };
            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            // Assign Admin Role
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = adminUserId,
                    RoleId = adminRoleId
                }
            );
        }
    }


}