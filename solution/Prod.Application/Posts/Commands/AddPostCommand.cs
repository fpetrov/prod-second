using MediatR;
using Prod.Application.Common.Interfaces.Persistence;
using Prod.Application.Common.Interfaces.Services;
using Prod.Application.Posts.Common;
using Prod.Domain.Entities;

namespace Prod.Application.Posts.Commands;

public class AddPostCommandHandler(
    IUserRepository userRepository,
    IPostRepository postRepository,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<AddPostCommand, PostResult>
{
    public async Task<PostResult> Handle(AddPostCommand request, CancellationToken cancellationToken)
    {
        var author = await userRepository
            .GetByLogin(request.Login, cancellationToken);
        
        var post = Post.Create(
            Guid.NewGuid(),
            request.Content,
            request.Tags,
            author!,
            dateTimeProvider.UtcNow);
        
        await postRepository.Add(post, cancellationToken);
        
        return new PostResult(
            post.Uuid,
            post.Content,
            post.Tags,
            post.Author.Login,
            post.CreatedAt,
            post.LikesCount,
            post.DislikesCount);
    }
}

public record AddPostCommand(
    string Login,
    string Content,
    List<string> Tags)
    : IRequest<PostResult>;