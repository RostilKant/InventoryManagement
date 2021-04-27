using Entities.Configuration;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<License> Licenses { get; set; }    
        public DbSet<Device> Devices { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Accessory> Accessories { get; set; }
        public DbSet<Consumable> Consumables { get; set; }  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Licenses)
                .WithMany(l => l.Employees);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Devices)
                .WithOne(d => d.Employee)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

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

            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceConfiguration());
        }
    }
}