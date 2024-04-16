namespace Prod.Domain.Common.RegexMatches;

public static partial class RegexMatches
{
    public static class User
    {
        public const string Login = @"^[a-zA-Z0-9-]{1,30}$";
        public const string Password = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,100}$";
        public const string CountryCode = @"^[a-zA-Z]{2}$";
        public const string Phone = @"\+[\d]+$";
    }
}