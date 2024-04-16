using Prod.Domain.Entities;

namespace Prod.Application.Common.Interfaces.Persistence;

public interface IPostRepository
{
    public Task Add(
        Post post,
        CancellationToken cancellationToken = default);
    
    public Task<Post?> GetByUuid(
        Guid uuid,
        CancellationToken cancellationToken = default);
    
    public Task<List<Post>> GetByAuthor(
        string login,
        int limit,
        int offset,
        CancellationToken cancellationToken = default);
    
    public Task Update(
        Post post,
        CancellationToken cancellationToken = default);
}