namespace Prod.Contracts.Authentication;

public record LoginRequest(
    string Login,
    string Password);