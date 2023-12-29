using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Entities.Configuration;
using Entities.IdentityModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class ApplicationContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public string TenantIdentifier { get; set; }
        private readonly ITenantManagerService _tenantService;

        public ApplicationContext(DbContextOptions options, ITenantManagerService tenantService)
            : base(options)
        {
            _tenantService = tenantService;
            TenantIdentifier = _tenantService.GetTenant()?.Id;
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<License> Licenses { get; set; }    
        public DbSet<Device> Devices { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Accessory> Accessories { get; set; }
        public DbSet<Consumable> Consumables { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantConnectionString = _tenantService.GetConnection();
            
            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                var dbProvider = _tenantService.GetDbProvider();

                switch (dbProvider.ToLower())
                {
                    case "mssql":
                        optionsBuilder.UseSqlServer(_tenantService.GetConnection(),
                            builder => builder.MigrationsAssembly("InventoryManagement"));
                        break;
                    case "postgres":
                        optionsBuilder.UseNpgsql(_tenantService.GetConnection(),
                            builder => builder.MigrationsAssembly("InventoryManagement"));
                        break;
                    default:
                        throw new Exception("Invalid database provider!");
                }
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Licenses)
                .WithMany(l => l.Employees);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Devices)
                .WithOne(d => d.Employee)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Employee>()
                .HasQueryFilter(a =>
                    a.TenantId == TenantIdentifier);

            modelBuilder.Entity<Device>()
                .HasMany(d => d.Accessories)
                .WithOne(a => a.Device)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<Device>()
                .HasMany(d => d.Components)
                .WithOne(a => a.Device)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<Device>()
                .HasMany(d => d.Consumables)
                .WithOne(a => a.Device)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Device>()
                .Property(d => d.Status)
                .HasConversion<string>();
            
            modelBuilder.Entity<Device>()
                .Property(d => d.Category)
                .HasConversion<string>();

            modelBuilder.Entity<Device>()
                .HasQueryFilter(a =>
                    a.TenantId == TenantIdentifier);
            
            modelBuilder.Entity<Employee>()
                .Property(e => e.Department)
                .HasConversion<string>();
            
            modelBuilder.Entity<License>()
                .Property(l => l.Category)
                .HasConversion<string>();

            modelBuilder.Entity<License>()
                .HasQueryFilter(a =>
                    a.TenantId == TenantIdentifier);
            
            modelBuilder.Entity<Component>()
                .Property(c => c.Category)
                .HasConversion<string>();
            
            modelBuilder.Entity<Component>()
                .Property(c => c.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Component>()
                .HasQueryFilter(a =>
                    a.TenantId == TenantIdentifier);
            
            modelBuilder.Entity<Consumable>()
                .Property(c => c.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Consumable>()
                .Property(c => c.Category)
                .HasConversion<string>();

            modelBuilder.Entity<Consumable>()
                .HasQueryFilter(a =>
                    a.TenantId == TenantIdentifier);
            
            modelBuilder.Entity<Accessory>()
                .Property(a => a.Category)
                .HasConversion<string>();
            
            modelBuilder.Entity<Accessory>()
                .Property(a => a.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Accessory>()
                .HasQueryFilter(a =>
                    a.TenantId == TenantIdentifier);
            
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<ITenantable>().ToList())
                entry.Entity.TenantId = entry.State switch
                {
                    EntityState.Added => TenantIdentifier,
                    EntityState.Modified => TenantIdentifier,
                    _ => entry.Entity.TenantId
                };

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}