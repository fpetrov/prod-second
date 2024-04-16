using System.Text.Json.Serialization;

namespace Prod.Contracts.Authentication;

public record GetMeResponse(
    string Login,
    string Email,
    string CountryCode,
    bool IsPublic,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string Phone,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string Image);