using ErrorOr;
using MediatR;
using Prod.Application.Authentication.Common;
using Prod.Application.Common.Interfaces.Authentication;
using Prod.Application.Common.Interfaces.Persistence;
using Prod.Domain.Common.Errors;

namespace Prod.Application.Authentication.Commands;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtTokenGenerator tokenGenerator,
    IPasswordVerifier passwordVerifier)
    : IRequestHandler<LoginCommand, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var (login, password) = request;

        var user = await userRepository.GetByLogin(login, cancellationToken);
        var userExists = user != null;

        if (!userExists)
            return Errors.User.NotFound();

        // If password is incorrect.
        if (!passwordVerifier.Verify(password, user!.Password))
            return Errors.Authentication.InvalidCredentials();

        var token = tokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
            login,
            user.Email,
            user.CountryCode,
            user.IsPublic,
            user.Phone,
            user.Image,
            token);

    }
}

public record LoginCommand(
    string Login,
    string Password)
    : IRequest<ErrorOr<AuthenticationResult>>;