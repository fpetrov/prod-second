using System.Text.RegularExpressions;
using FluentValidation;
using Prod.Domain.Common.RegexMatches;

namespace Prod.Application.Authentication.Commands;

public class LoginCommandValidator 
    : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(30)
            .Must(BeValidLogin);
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(100)
            .Must(BeValidPassword);
    }
    
    private static bool BeValidPassword(string password)
        => Regex.IsMatch(password, RegexMatches.User.Password);
    
    private static bool BeValidLogin(string login)
        => Regex.IsMatch(login, RegexMatches.User.Login);
}