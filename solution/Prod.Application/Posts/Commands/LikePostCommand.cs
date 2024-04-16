using ErrorOr;
using MediatR;
using Prod.Application.Common.Interfaces.Persistence;
using Prod.Application.Posts.Common;
using Prod.Application.Posts.Queries;
using Prod.Domain.Common.Errors;

namespace Prod.Application.Posts.Commands;

public class LikePostCommandHandler(
    ISender sender,
    IUserRepository userRepository,
    IPostRepository postRepository)
    : IRequestHandler<LikePostCommand, ErrorOr<PostResult>>
{
    public async Task<ErrorOr<PostResult>> Handle(
        LikePostCommand request,
        CancellationToken cancellationToken)
    {
        var (login, postId) = request;

        var query = new GetPostQuery(login, postId);

        var result = await sender.Send(
            query,
            cancellationToken);

        if (result.IsError)
            return Errors.Post.NotFound;
        
        var post = await postRepository.GetByUuid(
            postId,
            cancellationToken);

        var user = await userRepository
            .GetByLogin(login, cancellationToken);

        // Если у пользователя уже есть дизлайк на этом посте
        if (user.DislikedPostsIds.Contains(postId))
        {
            user.DislikedPostsIds.Remove(postId);
            post.DislikesCount--;
        }

        if (user.LikedPostsIds.Contains(postId))
            goto postResult;
        
        post.LikesCount++;
        user.LikedPostsIds.Add(postId);

        await postRepository.Update(
            post,
            cancellationToken);

        await userRepository.Update(
            user,
            cancellationToken);

        postResult:
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

public record LikePostCommand(
    string Login,
    Guid PostId)
    : IRequest<ErrorOr<PostResult>>;