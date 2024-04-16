using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Prod.Application.Common.Interfaces.Persistence;
using Prod.Domain.Entities;

namespace Prod.Infrastructure.Persistence;

public class RelationRepository(
    ApplicationContext context)
    : IRelationRepository
{
    public async Task Add(Relation relation, CancellationToken cancellationToken = default)
    {
        var alreadyContains = await ContainsRelation(relation);

        if (alreadyContains)
            return;

        context
            .Set<Relation>()
            .Add(relation);

        await context
            .SaveChangesAsync(cancellationToken);
    }

    public Task<bool> IsFriend(User user, User friend, CancellationToken cancellationToken = default)
    {
        if (user.Login == friend.Login)
            return Task.FromResult(true);
        
        return context
            .Set<Relation>()
            .AnyAsync(
                r => r.User.Login == user.Login
                     && r.Friend.Login == friend.Login,
                cancellationToken);
    }

    private Task<bool> ContainsRelation(Relation relation)
    {
        return context
            .Set<Relation>()
            .AnyAsync(
                r => r.User.Login == relation.User.Login
                     && r.Friend.Login == relation.Friend.Login);
    }

    public Task Remove(Relation relation, CancellationToken cancellationToken = default)
    {
        context
            .Set<Relation>()
            .Remove(relation);

        return context.SaveChangesAsync(cancellationToken);
    }

    public IQueryable<Relation> QueryBy(Expression<Func<Relation, bool>> expression)
        => context
            .Set<Relation>()
            .Where(expression);
    
    public async Task<bool> Remove(
        string login,
        string friendLogin,
        CancellationToken cancellationToken = default)
    {
        var relation = await context
            .Set<Relation>()
            .FirstOrDefaultAsync(r =>
                    r.User.Login == login &&
                    r.Friend.Login == friendLogin,
                cancellationToken: cancellationToken);

        if (relation is null)
            return true;

        await Remove(relation, cancellationToken);
        
        return true;
    }
}