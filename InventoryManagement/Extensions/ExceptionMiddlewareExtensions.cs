using System.Net;
using Entities.ErrorModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger<Startup> logger)
        {
            app.UseExceptionHandler(appError => 
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextExceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextExceptionFeature != null)
                    {
                        var error = contextExceptionFeature.Error;
                        logger.LogError("Something went wrong: {Error}",error);

                        await context.Response.WriteAsync(new GlobalError
                        {
                            Message = "Internal Server Error",
                            StatusCode = context.Response.StatusCode
                        }.ToString());
                    }
                }));
        }
    }
}