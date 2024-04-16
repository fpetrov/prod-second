using System.Reflection;
using Mapster;
using MapsterMapper;

namespace Prod.Api.Common.Mappings;

public static class DependencyInjection
{
    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        
        return services;
    }
}