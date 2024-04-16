using MediatR;
using Prod.Application.Authentication.Common;
using Prod.Application.Authentication.Queries;

namespace Prod.Api.Middlewares;

/// <summary>
/// Used to authorize user from Bearer token.  
/// </summary>
public class AuthorizationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        ISender sender)
    {
        if (!RequiresAuthorization(context))
            goto skip;
        
        var login = context.User.FindFirst(AuthenticationResult.LoginClaim)?.Value;

        if (!string.IsNullOrEmpty(login))
        {
            var ticks = long.Parse(context.User.FindFirst("passwordChangeDate")?.Value!);
            var passwordChangeDate = new DateTime()
                .AddTicks(ticks);
            
            var request = new GetMeQuery(login, passwordChangeDate);
            
            var result = await sender.Send(request);

            result.MatchFirst(
                authResult => AppendAuthenticationResult(context, authResult),
                _ =>
                {
                    context.Response.StatusCode = 401;
                    return false;
                });
        }
        
        skip:
        await next(context);
    }
    
    private static bool RequiresAuthorization(HttpContext context)
    {
        return context.Request.Headers.Authorization.Count > 0;
    }

    // Append authentication result (user' fields) to HttpContext item collection.
    private static bool AppendAuthenticationResult(
        HttpContext context,
        AuthenticationResult result)
    {
        context.Request.HttpContext.Items.Add(
            AuthenticationResult.Key,
            result);

        return true;
    }
}