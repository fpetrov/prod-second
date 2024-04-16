using ErrorOr;
using MediatR;
using Prod.Application.Common.Interfaces.Persistence;
using Prod.Application.Posts.Common;
using Prod.Domain.Common.Errors;

namespace Prod.Application.Posts.Queries;

public class GetPostQueryHandler(
    IPostRepository postRepository,
    IRelationRepository relationRepository,
    IUserRepository userRepository)
    : IRequestHandler<GetPostQuery, ErrorOr<PostResult>>
{
    public async Task<ErrorOr<PostResult>> Handle(
        GetPostQuery request,
        CancellationToken cancellationToken)
    {
        var (login, uuid) = request;
        
        var post = await postRepository
            .GetByUuid(uuid, cancellationToken);

        if (post is null)
            return Errors.Post.NotFound;

        var author = post.Author;
        var user = await userRepository.GetByLogin(login, cancellationToken);
        
        var isFriend = await relationRepository
            .IsFriend(author, user!, cancellationToken);

        if (!author.IsPublic && !isFriend)
            return Errors.Post.NotFound;
        
        return new PostResult(
            post.Uuid,
            post.Content,
            post.Tags,
            author.Login,
            post.CreatedAt,
            post.LikesCount,
            post.DislikesCount);
    }
}

public record GetPostQuery(
    string Login,
    Guid Uuid)
    : IRequest<ErrorOr<PostResult>>;