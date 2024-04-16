using System.Linq.Expressions;
using Prod.Domain.Entities;

namespace Prod.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    public Task<User?> GetByLogin(
        string login,
        CancellationToken cancellationToken = default);
    public Task<User?> GetByEmail(
        string email,
        CancellationToken cancellationToken = default);
    public Task<User?> GetByPhone(
        string? phone,
        CancellationToken cancellationToken = default);

    public Task Add(
        User user,
        CancellationToken cancellationToken = default);
    
    public Task Update(
        User user,
        CancellationToken cancellationToken = default);

    public IQueryable<User> QueryBy(Expression<Func<User, bool>> predicate);
}