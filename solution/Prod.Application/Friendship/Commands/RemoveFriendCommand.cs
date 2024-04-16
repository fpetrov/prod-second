using MediatR;
using Prod.Application.Common.Interfaces.Persistence;

namespace Prod.Application.Friendship.Commands;

public class RemoveFriendCommandHandler(
    IUserRepository userRepository,
    IRelationRepository relationRepository)
    : IRequestHandler<RemoveFriendCommand, bool>
{
    public async Task<bool> Handle(RemoveFriendCommand request, CancellationToken cancellationToken)
    {
        var (login, friendLogin) = request;

        var relation = await relationRepository
            .Remove(login, friendLogin, cancellationToken);

        return relation;
    }
}

public record RemoveFriendCommand(
    string Login,
    string FriendLogin)
    : IRequest<bool>;