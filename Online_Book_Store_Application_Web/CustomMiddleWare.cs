using Microsoft.AspNetCore.Mvc.Filters;

namespace Online_Book_Store_Application_Web
{
    public class CustomMiddleWare : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Response.Headers.Remove("X-Powered-By");
            context.HttpContext.Response.Headers.Remove("Server");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Do nothing.
        }
    }
}
