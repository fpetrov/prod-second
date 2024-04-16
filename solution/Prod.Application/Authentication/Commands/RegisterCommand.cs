using ErrorOr;
using MediatR;
using Prod.Application.Authentication.Common;
using Prod.Application.Common.Interfaces.Authentication;
using Prod.Application.Common.Interfaces.Persistence;
using Prod.Application.Common.Interfaces.Services;
using Prod.Domain.Common.Errors;
using Prod.Domain.Entities;

namespace Prod.Application.Authentication.Commands;

public class RegisterCommandHandler(
    IUserRepository userRepository,
    ICountryRepository countryRepository,
    IJwtTokenGenerator tokenGenerator,
    IDateTimeProvider dateTimeProvider,
    IPasswordVerifier passwordVerifier)
    : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var (login, email, password, countryCode, phone, image, isPublic) = request;

        var country = await countryRepository
            .GetByAlpha2(countryCode, cancellationToken);
        
        if (country is null)
            return Errors.User.Validation();
        
        var duplicateLogin =
            (await userRepository.GetByLogin(login, cancellationToken)) != null;
        
        var duplicateEmail =
            (await userRepository.GetByEmail(email, cancellationToken)) != null;
        
        var duplicatePhone =
            (await userRepository.GetByPhone(phone, cancellationToken)) != null;

        // TODO: Add validation for Alpha2 Code.
        
        if (duplicateLogin ||
            duplicateEmail ||
            duplicatePhone)
            return Errors.User.DuplicateData();
        
        var user = new User
        {
            Login = login,
            Email = email,
            Password = passwordVerifier.Hash(password),
            CountryCode = countryCode,
            Phone = phone,
            Image = image,
            IsPublic = isPublic,
            PasswordChangeDate = dateTimeProvider.UtcNow
        };

        await userRepository.Add(user, cancellationToken);
        
        var token = tokenGenerator.GenerateToken(user);
        
        return new AuthenticationResult(
            login,
            email,
            countryCode,
            isPublic,
            phone,
            image,
            token);
    }
}

public record RegisterCommand(
    string Login,
    string Email,
    string Password,
    string CountryCode,
    string? Phone,
    string? Image,
    bool IsPublic)
    : IRequest<ErrorOr<AuthenticationResult>>;