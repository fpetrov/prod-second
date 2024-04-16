using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Prod.Application.Common.Interfaces.Authentication;
using Prod.Application.Common.Interfaces.Persistence;
using Prod.Application.Common.Interfaces.Services;
using Prod.Infrastructure.Authentication;
using Prod.Infrastructure.Persistence;
using Prod.Infrastructure.Services;

namespace Prod.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddPersistence(configuration);
        services.AddDefaultAuthentication(configuration);
        
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IPasswordVerifier, PasswordVerifier>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IRelationRepository, RelationRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        
        return services;
    }

    private static IServiceCollection AddDefaultAuthentication(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        
        services
            .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Secret))
            });

        services.AddAuthorization();
        
        return services;
    }
}