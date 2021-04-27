using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.ActionFilters
{
    public class ValidationAsyncFilterAttribute : IAsyncActionFilter
    {
        private readonly ILogger<ValidationAsyncFilterAttribute> _logger;

        public ValidationAsyncFilterAttribute(ILogger<ValidationAsyncFilterAttribute> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];
            
            var param = context.ActionArguments
                .SingleOrDefault(x => x.Value.ToString().Contains("Dto")).Value;
            
            if (param.Equals(null))
            {
                _logger.LogError($"Object sent from client is null. Controller: {controller}, action: {action}");
                context.Result = new BadRequestObjectResult($"Object is null. Controller: {controller}, action: {action}");
                return;
            }
            
            if (!context.ModelState.IsValid)
            {
                _logger.LogError($"Invalid model state for the object. Controller:{controller}, action: {action}");
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            }

            var result = await next();
        }
    }
}