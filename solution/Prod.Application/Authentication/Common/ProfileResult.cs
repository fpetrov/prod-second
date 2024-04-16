namespace Prod.Application.Authentication.Common;

public record ProfileResult(
    string Login,
    string Email,
    string CountryCode,
    bool IsPublic,
    string? Phone,
    string? Image);