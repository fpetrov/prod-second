namespace Prod.Application.Common.Interfaces.Authentication;

public interface IPasswordVerifier
{
    public bool Verify(string password, string hash);
    public string Hash(string password);
}