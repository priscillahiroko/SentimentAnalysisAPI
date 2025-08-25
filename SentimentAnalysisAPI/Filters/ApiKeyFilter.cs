using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace SentimentAnalysisAPI.Filters
{
    public class ApiKeyFilter : IActionFilter
    {
        private readonly string _apiKey;

        public ApiKeyFilter(IConfiguration configuration)
        {
            _apiKey = configuration["ApiSettings:ApiKey"];
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("x-api-key", out var incomingApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (incomingApiKey != _apiKey)
            {
                context.Result = new UnauthorizedResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Não faz nada após a ação
        }
    }
}
