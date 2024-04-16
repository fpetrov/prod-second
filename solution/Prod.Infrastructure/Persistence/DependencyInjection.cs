using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Prod.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        ConfigurationManager configuration)
    { 
         // var connectionString = configuration.GetConnectionString("DefaultConnection");
        
         var postgresPort = configuration["POSTGRES_PORT"];
         var postgresHost = configuration["POSTGRES_HOST"];
         var postgresUser = configuration["POSTGRES_USERNAME"];
         var postgresPassword = configuration["POSTGRES_PASSWORD"];
         var postgresDatabase = configuration["POSTGRES_DATABASE"];
         
         var connectionString = $"Host={postgresHost};Port={postgresPort};Database={postgresDatabase};Username={postgresUser};Password={postgresPassword};";
        
        services
            .AddDbContext<ApplicationContext>(options 
                => options.UseNpgsql(connectionString));
        
        return services;
    }
}


/*var postgresPort = Environment.GetEnvironmentVariable("POSTGRES_PORT");
var postgresHost = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
var postgresUser = Environment.GetEnvironmentVariable("POSTGRES_USERNAME");
var postgresPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
var postgresDatabase = Environment.GetEnvironmentVariable("POSTGRES_DATABASE");

var connectionString = $"Host={postgresHost};Port={postgresPort};Database={postgresDatabase};Username={postgresUser};Password={postgresPassword};";
        
builder.Services
    .AddDbContext<ApplicationContext>(options 
        => options.UseNpgsql(connectionString));*/