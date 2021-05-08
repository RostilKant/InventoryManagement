using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Repository.Contracts;
using Services;
using Services.Contracts;

namespace InventoryManagement.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CORS", builder =>
                        builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                );
            });

        public static void ConfigureSqlContext(this IServiceCollection services, 
            IConfiguration configuration) =>
            services.AddDbContext<ApplicationContext>(options => 
                options.UseNpgsql(configuration.GetConnectionString("SQLConnection"), 
                    builder => builder.MigrationsAssembly("InventoryManagement")));

        public static void ConfigureRepositoryManager(this IServiceCollection services)
            => services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<IAccessoryService, AccessoryService>();
            services.AddScoped<IComponentService, ComponentService>();
            services.AddScoped<IConsumableService, ConsumableService>();
            services.AddScoped<ILicenseService, LicenseService>();
        }
    }
}