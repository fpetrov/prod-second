namespace Prod.Contracts.Authentication;

public record UpdateMeRequest(
    string? CountryCode,
    bool? IsPublic,
    string? Phone,
    string? Image);