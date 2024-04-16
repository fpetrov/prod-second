using Prod.Application.Authentication.Common;

namespace Prod.Api.Common.Extensions;

public static class HttpContextExtensions
{
    public static bool TryGet<TItem>(
        this HttpContext context,
        string key,
        out TItem? result)
    {
        if (context.Items.TryGetValue(key, out var item) &&
            item is TItem value)
        {
            result = value;
            return true;
        }

        result = default;
        return false;
    }
    
    public static AuthenticationResult? GetIdentity(
        this HttpContext context)
    {
        return context.TryGet<AuthenticationResult>(
            AuthenticationResult.Key,
            out var result)
            ? result
            : null;
    }
}