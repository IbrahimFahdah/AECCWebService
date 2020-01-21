using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Plugin.Abstraction
{
    public class Tracker : ActionFilterAttribute
    {
        private readonly ILogger _logger;

        public Tracker( string controllerName, ILoggerFactory logger)
        {
            _logger = logger.CreateLogger("AECC." + controllerName);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            string actionName = context.RouteData.Values["action"].ToString();
            _logger.Log(LogLevel.Information, $"IP:{context.HttpContext.Connection.RemoteIpAddress} Action:{actionName}");
            base.OnResultExecuting(context);
        }
    }
}