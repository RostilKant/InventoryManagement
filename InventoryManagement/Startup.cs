using AspNetCoreRateLimit;
using Entities.Settings;
using InventoryManagement.ActionFilters;
using InventoryManagement.Extensions;
using InventoryManagement.Messaging;
using InventoryManagement.Messaging.Consumers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InventoryManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddSwaggerGenNewtonsoftSupport();

            // use http context accessor before services registration
            services.ConfigureHttpContextAccessor();
            services.ConfigureServices();
            services.ConfigureSettings(Configuration);
            services.ConfigureCors();
            services.AddAndMigrateTenantDatabases();
            services.ConfigureRepositoryManager();
            services.AddAutoMapper(typeof(Startup));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddScoped<ValidationFilterAttribute>();
            services.AddMemoryCache();
            
            // services.ConfigureRateLimitingOptions();
            
            services.AddAuthentication();
            services.ConfigureIdentity();
            services.ConfigureJwt(Configuration);
            services.ConfigureSwagger();
            
            //Add rabbitMQ
            services.AddMassTransitRabbitMQ(Configuration.GetSection("RabbitMQConnection"))
                .AddConsumer<NewTenantConsumer>()
                .Build();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(config =>
                {
                    config.SwaggerEndpoint("/swagger/v1/swagger.json", "InventoryManagement v1");
                    config.RoutePrefix = string.Empty;
                });
            }
            else
            {
                app.UseHsts();
            }
            
            app.ConfigureExceptionHandler(logger);
            
            app.UseHttpsRedirection();
            
            app.UseStaticFiles();
            
            app.UseCors("CORS");

            // app.UseIpRateLimiting();
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}