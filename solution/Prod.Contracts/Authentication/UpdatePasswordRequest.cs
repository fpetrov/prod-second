namespace Prod.Contracts.Authentication;

public record UpdatePasswordRequest(
    string OldPassword,
    string NewPassword);