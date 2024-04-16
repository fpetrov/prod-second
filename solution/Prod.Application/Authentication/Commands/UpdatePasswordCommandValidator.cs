using System.Text.RegularExpressions;
using FluentValidation;
using Prod.Domain.Common.RegexMatches;

namespace Prod.Application.Authentication.Commands;

public class UpdatePasswordCommandValidator
    : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(100)
            .Must(BeValidPassword);
        
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(100)
            .Must(BeValidPassword);
    }
    
    private static bool BeValidPassword(string password)
        => Regex.IsMatch(password, RegexMatches.User.Password);
}