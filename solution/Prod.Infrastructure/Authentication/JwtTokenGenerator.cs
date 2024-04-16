using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Prod.Application.Common.Interfaces.Authentication;
using Prod.Application.Common.Interfaces.Services;
using Prod.Domain.Entities;

namespace Prod.Infrastructure.Authentication;

public class JwtTokenGenerator(
    IDateTimeProvider dateTimeProvider,
    IOptions<JwtSettings> options)
    : IJwtTokenGenerator
{
    private readonly JwtSettings _options = options.Value;
    
    public string GenerateToken(User user)
    {
        var secretKey = Encoding.UTF8.GetBytes(_options.Secret);
        
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(secretKey),
            SecurityAlgorithms.HmacSha256);
        
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var claims = new[]
        {
            new Claim("sub", user.Id.ToString()),
            new Claim("login", user.Login),
            new Claim("passwordChangeDate", user.PasswordChangeDate.Ticks.ToString())
        };

        var securityToken = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            expires: dateTimeProvider.UtcNow.AddMinutes(_options.DurationInMinutes),
            claims: claims,
            signingCredentials: signingCredentials);

        return tokenHandler.WriteToken(securityToken);
    }
}