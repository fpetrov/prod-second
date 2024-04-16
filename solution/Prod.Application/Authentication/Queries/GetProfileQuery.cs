using ErrorOr;
using MediatR;
using Prod.Application.Authentication.Common;
using Prod.Application.Common.Interfaces.Persistence;
using Prod.Domain.Common.Errors;

namespace Prod.Application.Authentication.Queries;

public class GetProfileQueryHandler(
    IUserRepository userRepository)
    : IRequestHandler<GetProfileQuery, ErrorOr<ProfileResult>>
{
    public async Task<ErrorOr<ProfileResult>> Handle(
        GetProfileQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository
            .GetByLogin(request.Login, cancellationToken);
        
        var requestedUser = await userRepository
            .GetByLogin(request.RequestedLogin, cancellationToken);
        
        if (user is null || requestedUser is null)
            return Errors.User.NotFound();

        if (requestedUser.Login == request.Login)
            goto profileResult;
        
        if (!requestedUser.IsPublic)
            return Errors.User.NotFound();
        
        profileResult:
        // TODO: Maybe add mapper?
        return new ProfileResult(
            Login: requestedUser.Login,
            Email: requestedUser.Email,
            CountryCode: requestedUser.CountryCode,
            IsPublic: requestedUser.IsPublic,
            Phone: requestedUser.Phone,
            Image: requestedUser.Image);
    }
}

public record GetProfileQuery(
    string Login,
    string RequestedLogin)
    : IRequest<ErrorOr<ProfileResult>>;