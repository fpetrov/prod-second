using System.Text.RegularExpressions;
using FluentValidation;
using Prod.Domain.Common.RegexMatches;

namespace Prod.Application.Authentication.Commands;

public class RegisterCommandValidator
    : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(30)
            .Must(BeValidLogin);
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(50);
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(100)
            .Must(BeValidPassword);

        RuleFor(x => x.CountryCode)
            .NotEmpty()
            .Length(2)
            .Must(BeValidCountryCode);

        RuleFor(x => x.IsPublic)
            .NotNull();

        RuleFor(x => x.Phone)
            .Must(BeValidPhone)
            .MinimumLength(1)
            .MaximumLength(20)
            .When(x => x.Phone is not null);

        RuleFor(x => x.Image)
            .MinimumLength(1)
            .MaximumLength(200)
            .When(x => x.Image is not null);
    }
    
    private static bool BeValidPhone(string? phone)
        => Regex.IsMatch(phone!, RegexMatches.User.Phone);
    
    private static bool BeValidCountryCode(string countryCode)
        => Regex.IsMatch(countryCode, RegexMatches.User.CountryCode);
    
    private static bool BeValidPassword(string password)
        => Regex.IsMatch(password, RegexMatches.User.Password);
    
    private static bool BeValidLogin(string login)
        => Regex.IsMatch(login, RegexMatches.User.Login);
}