using Prod.Api.Common.Mappings;

namespace Prod.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services)
    {
        services.AddMapping();
        
        return services;
    }
}