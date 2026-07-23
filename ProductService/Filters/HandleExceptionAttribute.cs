using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProductService.Filters;

public class HandleExceptionAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        context.Result = new ObjectResult(new
        {
            Error = "An error occurred while processing your request.",
            Detail = context.Exception.Message
        })
        {
            StatusCode = 500
        };
        context.ExceptionHandled = true;
    }
}