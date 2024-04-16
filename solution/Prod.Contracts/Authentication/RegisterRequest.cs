using System.Text.Json.Serialization;

namespace Prod.Contracts.Authentication;

public record RegisterRequest(string Login,
    string Email,
    string Password,
    string CountryCode,
    bool IsPublic,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Image = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Phone = null);