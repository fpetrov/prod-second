namespace Prod.Api.Filters;

public class ErrorReasonFilter : IEndpointFilter
{
    private static readonly List<int> ErrorCodes = [
        400, 401, 403, 404, 409, 405
    ];

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var result = await next(context);

        return IsErrorOccured(result, out var statusCode)
            ? Results.Json(
                new { Reason = "Something went wrong, it may be validation error." },
                statusCode: statusCode)
            : result;
    }

    private static bool IsErrorOccured(object? result, out int statusCode)
    {
        if (result is IStatusCodeHttpResult response)
        {
            statusCode = response.StatusCode!.Value;
            return ErrorCodes.Contains(response.StatusCode!.Value);
        }

        statusCode = 400;
        return false;
    }
}