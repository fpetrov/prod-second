using ErrorOr;
using MediatR;
using Prod.Application.Authentication.Common;
using Prod.Application.Common.Interfaces.Authentication;
using Prod.Application.Common.Interfaces.Persistence;
using Prod.Application.Common.Interfaces.Services;
using Prod.Domain.Common.Errors;

namespace Prod.Application.Authentication.Commands;

public class UpdatePasswordCommandHandler(
    IUserRepository userRepository,
    IPasswordVerifier passwordVerifier,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<UpdatePasswordCommand, ErrorOr<ProfileResult>>
{
    public async Task<ErrorOr<ProfileResult>> Handle(
        UpdatePasswordCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository
            .GetByLogin(request.Login, cancellationToken);

        if (user is null)
            return Errors.User.NotFound();
        
        // If password is NOT correct.
        if (!passwordVerifier.Verify(
                request.OldPassword, user.Password))
            return Errors.User.WrongPassword();
        
        user.Password = passwordVerifier.Hash(request.NewPassword);
        user.PasswordChangeDate = dateTimeProvider.UtcNow;
        
        await userRepository
            .Update(user, cancellationToken);
        
        return new ProfileResult(
            Login: user.Login,
            Email: user.Email,
            CountryCode: user.CountryCode,
            IsPublic: user.IsPublic,
            Phone: user.Phone,
            Image: user.Image);
    }
}

public record UpdatePasswordCommand(
    string Login,
    string OldPassword,
    string NewPassword)
    : IRequest<ErrorOr<ProfileResult>>;