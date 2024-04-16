namespace Prod.Application.Authentication.Common;

public record AuthenticationResult(
    string Login,
    string Email,
    string CountryCode,
    bool IsPublic,
    string? Phone,
    string? Image,
    string Token)
{
    public const string LoginClaim = "login";
    public const string Key = "auth";
}