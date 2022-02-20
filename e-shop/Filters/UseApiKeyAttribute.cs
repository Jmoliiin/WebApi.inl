using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace e_shop.Filters
{
    [AttributeUsage(validOn:AttributeTargets.Class | AttributeTargets.Method)]
    public class UseApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = configuration.GetValue<string>("ApiKey");

            //Quary - urlen
            if (!context.HttpContext.Request.Query.TryGetValue("code", out var providedCode))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!apiKey.Equals(providedCode))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}
