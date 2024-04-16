namespace Prod.Api.Middlewares;

public class GlobalErrorHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private static Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var code = StatusCodes.Status400BadRequest;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = code;
        
        return context.Response.WriteAsJsonAsync(new
        {
            Reason = "Something went wrong."
        });
    }
}