using MediatR;
using Microsoft.EntityFrameworkCore;
using Prod.Application.Common.Interfaces.Persistence;
using Prod.Domain.Entities;

namespace Prod.Application.Friendship.Queries;

public class GetFriendsQueryHandler(
    IUserRepository userRepository,
    IRelationRepository relationRepository)
    : IRequestHandler<GetFriendsQuery, List<Relation>>
{
    public async Task<List<Relation>> Handle(
        GetFriendsQuery request,
        CancellationToken cancellationToken)
    {
        var (login, limit, offset) = request;
        
        var user = await userRepository
            .GetByLogin(login, cancellationToken);
        
        limit = FixValue(limit, 0, 50);
        offset = Math.Max(offset, 0);
        
        // TODO: Add Specification Pattern.
        var relations = await relationRepository
            .QueryBy(r => r.User.Id == user.Id)
            .OrderByDescending(r => r.StartedAt)
            .Skip(offset)
            .Take(limit)
            .Include(t => t.Friend)
            .ToListAsync(cancellationToken: cancellationToken);

        return relations;
    }

    private static int FixValue(
        int value,
        int minValue,
        int maxValue)
        => Math.Min(Math.Max(value, minValue), maxValue);
}

public record GetFriendsQuery(
    string Login,
    int Limit,
    int Offset)
    : IRequest<List<Relation>>;