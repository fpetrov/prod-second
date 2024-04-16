namespace Prod.Contracts.Authentication;

public record GetProfileResponse(
    string Login,
    string Email,
    string CountryCode,
    bool IsPublic,
    string? Phone,
    string? Image);