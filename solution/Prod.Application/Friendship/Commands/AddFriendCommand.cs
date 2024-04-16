using ErrorOr;
using MediatR;
using Prod.Application.Common.Interfaces.Persistence;
using Prod.Application.Common.Interfaces.Services;
using Prod.Domain.Common.Errors;
using Prod.Domain.Entities;

namespace Prod.Application.Friendship.Commands;

public class AddFriendCommandHandler(
    IUserRepository userRepository,
    IRelationRepository relationRepository,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<AddFriendCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(
        AddFriendCommand request,
        CancellationToken cancellationToken)
    {
        var (login, friendLogin) = request;

        // Если пользователь добавляет себя в друзья
        if (login.Equals(friendLogin))
            return true;

        var user = await userRepository
            .GetByLogin(login, cancellationToken);

        var friend = await userRepository
            .GetByLogin(friendLogin, cancellationToken);

        if (friend is null)
            return Errors.User.NotFound();
        
        var relation = Relation.Create(
            user!, friend, dateTimeProvider.UtcNow);

        await relationRepository.Add(relation, cancellationToken);
        
        return true;
    }
}

public record AddFriendCommand(
    string Login,
    string FriendLogin)
    : IRequest<ErrorOr<bool>>;