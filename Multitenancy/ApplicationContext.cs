using Microsoft.EntityFrameworkCore;

namespace Multitenancy
{
    public class ApplicationContext : DbContext
    {
        public string TenantIdentifier { get; set; }
        private readonly ITenantManagerService _tenantService;

        public ApplicationContext(DbContextOptions<ApplicationContext> options, ITenantManagerService tenantService)
            : base(options)
        {
            _tenantService = tenantService;
            TenantIdentifier = _tenantService.GetTenant()?.Id;
        }
        
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
            
            // implement fluent api
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