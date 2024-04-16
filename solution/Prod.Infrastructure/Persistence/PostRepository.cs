using Microsoft.EntityFrameworkCore;
using Prod.Application.Common.Interfaces.Persistence;
using Prod.Domain.Entities;

namespace Prod.Infrastructure.Persistence;

public class PostRepository(
    ApplicationContext context)
    : IPostRepository
{
    public async Task Add(Post post, CancellationToken cancellationToken = default)
    {
        await context.Posts.AddAsync(post, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    
    public Task<Post?> GetByUuid(Guid uuid, CancellationToken cancellationToken = default)
    {
        return context
            .Posts
            .Include(t => t.Author)
            .FirstOrDefaultAsync(x => x.Uuid == uuid, cancellationToken);
    }

    public Task<List<Post>> GetByAuthor(string login, int limit, int offset, CancellationToken cancellationToken = default)
    {
        return context
            .Posts
            .Where(t => t.Author.Login == login)
            .OrderByDescending(t => t.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .Include(t => t.Author)
            .ToListAsync(cancellationToken);
    }

    public Task Update(Post post, CancellationToken cancellationToken = default)
    {
        context
            .Entry(post)
            .State = EntityState.Modified;
        
        return context
            .SaveChangesAsync(cancellationToken);
    }
}