using Prod.Application.Common.Interfaces.Authentication;

namespace Prod.Infrastructure.Authentication;

public class PasswordVerifier : IPasswordVerifier
{
    public bool Verify(string password, string hash) 
        => BCrypt.Net.BCrypt.Verify(password, hash);

    public string Hash(string password) 
        => BCrypt.Net.BCrypt.HashPassword(password);
}