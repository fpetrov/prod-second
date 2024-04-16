using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Prod.Application.Authentication.Common;
using Prod.Application.Common.Interfaces.Authentication;
using Prod.Application.Common.Interfaces.Persistence;
using Prod.Domain.Common.Errors;

namespace Prod.Application.Authentication.Queries;

public class GetMeQueryHandler(
    IUserRepository userRepository,
    IJwtTokenGenerator tokenGenerator
    )
    : IRequestHandler<GetMeQuery, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(
        GetMeQuery request,
        CancellationToken cancellationToken)
    {
        var (login, lastPasswordChangeDate) = request;
        
        var user = await userRepository.GetByLogin(login, cancellationToken);
        
        if (user is null)
            return Errors.Authentication.InvalidCredentials();
        
        // WARNING: Actually it's not safe. Last password change date could be changed in token.
        // That's why people often use Redis to store deactivated tokens.
        if (user.PasswordChangeDate > lastPasswordChangeDate)
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

/// <summary>
/// Request used by <see cref="AuthorizationMiddleware"/>
/// to get current user.
/// </summary>
/// <param name="Login"></param>
/// <param name="LastPasswordChangeDate"></param>
public record GetMeQuery(
    string Login,
    DateTime LastPasswordChangeDate)
    : IRequest<ErrorOr<AuthenticationResult>>;