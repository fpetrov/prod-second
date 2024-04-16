using System.Linq.Expressions;
using Prod.Domain.Entities;

namespace Prod.Application.Common.Interfaces.Persistence;

public interface IRelationRepository
{
    public Task Add(
        Relation relation,
        CancellationToken cancellationToken = default);
    
    public Task<bool> IsFriend(
        User user,
        User friend,
        CancellationToken cancellationToken = default);

    public Task Remove(
        Relation relation,
        CancellationToken cancellationToken = default);

    public IQueryable<Relation> QueryBy(
        Expression<Func<Relation, bool>> expression);
    
    public Task<bool> Remove(
        string login,
        string friendLogin,
        CancellationToken cancellationToken = default);
}