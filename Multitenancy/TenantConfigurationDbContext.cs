using Microsoft.EntityFrameworkCore;

namespace Multitenancy
{
    public class TenantConfigurationDbContext : DbContext
    {
        public TenantConfigurationDbContext(DbContextOptions<TenantConfigurationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tenant>()
                .HasKey(t => t.Id);
        }
    }
}