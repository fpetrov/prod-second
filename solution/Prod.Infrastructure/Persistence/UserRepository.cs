using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Prod.Application.Common.Interfaces.Persistence;
using Prod.Domain.Entities;

namespace Prod.Infrastructure.Persistence;

public class UserRepository(
    ApplicationContext context)
    : IUserRepository
{
    public Task<User?> GetByLogin(string login, CancellationToken cancellationToken = default)
    {
        return context
            .Set<User>()
            .Where(user => user.Login == login)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<User?> GetByEmail(string email, CancellationToken cancellationToken = default)
    {
        return context
            .Set<User>()
            .Where(user => user.Email == email)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<User?> GetByPhone(string? phone, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(phone))
            return Task.FromResult<User?>(null);
        
        return context
            .Set<User>()
            .Where(user => user.Phone == phone)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task Add(User user, CancellationToken cancellationToken = default)
    {
        var createdEntity = context
            .Set<User>()
            .Add(user);
        
        return context
            .SaveChangesAsync(cancellationToken);
    }

    public Task Update(User user, CancellationToken cancellationToken = default)
    {
        context
            .Entry(user)
            .State = EntityState.Modified;
        
        return context
            .SaveChangesAsync(cancellationToken);
    }

    public IQueryable<User> QueryBy(Expression<Func<User, bool>> predicate)
    {
        return context
            .Set<User>()
            .Where(predicate);
    }
}