using System.Text.RegularExpressions;
using FluentValidation;
using Prod.Domain.Common.RegexMatches;

namespace Prod.Application.Authentication.Commands;

public class UpdateMeCommandValidator
    : AbstractValidator<UpdateMeCommand>
{
    public UpdateMeCommandValidator()
    {
        RuleFor(x => x.CountryCode)
            .Length(2)
            .Must(BeValidCountryCode)
            .When(x => x.CountryCode is not null);

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
    
    private static bool BeValidCountryCode(string? countryCode)
        => Regex.IsMatch(countryCode!, RegexMatches.User.CountryCode);
}