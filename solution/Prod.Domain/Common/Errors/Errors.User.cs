using ErrorOr;

namespace Prod.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error WrongPassword() 
            => Error.Forbidden(
                code: "User.WrongPassword", 
                description: "Wrong password");
        
        public static Error NotFound() 
            => Error.NotFound(
                code: "User.NotFound", 
                description: "User not found");
        
        public static Error Validation()
            => Error.Validation(
                code: "User.Validation",
                description: "User validation error");
        
        public static Error DuplicateData()
            => Error.Conflict(
                code: "User.DuplicateData",
                description: "User with such data already exists");
    }
}