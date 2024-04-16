using System.Text.Json.Serialization;

namespace Prod.Contracts.Authentication;

public record RegisterResponse(
    string Login,
    string Email,
    string CountryCode,
    bool IsPublic,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Phone = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Image = null);