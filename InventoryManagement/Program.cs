using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace InventoryManagement
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            
            Log.Logger = new LoggerConfiguration()  
                .Enrich.FromLogContext()  
                .WriteTo.Console()
                .WriteTo.Seq("http://localhost:5341/")
                .MinimumLevel.Debug()
                .CreateLogger();  
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}