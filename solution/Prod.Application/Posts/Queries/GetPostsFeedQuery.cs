using ErrorOr;
using MediatR;
using Prod.Application.Common.Interfaces.Persistence;
using Prod.Application.Posts.Common;
using Prod.Domain.Common.Errors;

namespace Prod.Application.Posts.Queries;

public class GetPostsFeedQueryHandler(
    IPostRepository postRepository,
    IUserRepository userRepository,
    IRelationRepository relationRepository)
    : IRequestHandler<GetPostsFeedQuery, ErrorOr<List<PostResult>>>
{
    public async Task<ErrorOr<List<PostResult>>> Handle(
        GetPostsFeedQuery request,
        CancellationToken cancellationToken)
    {
        var (login, authorLogin, limit, offset) = request;

        var user = await userRepository
            .GetByLogin(login, cancellationToken);

        var author = await userRepository
            .GetByLogin(authorLogin, cancellationToken);

        if (author is null)
            return Errors.Post.NotFound;
        
        var isFriend = await relationRepository
            .IsFriend(author, user!, cancellationToken);
        
        if (!author.IsPublic && !isFriend)
            return Errors.Post.NotFound;
        
        var posts = await postRepository
            .GetByAuthor(authorLogin, limit, offset, cancellationToken);
        
        return posts
            .ConvertAll(t => new PostResult(
                t.Uuid,
                t.Content,
                t.Tags,
                t.Author.Login,
                t.CreatedAt,
                t.LikesCount,
                t.DislikesCount));
    }
}

public record GetPostsFeedQuery(
    string Login,
    string Author,
    int Offset,
    int Limit)
    : IRequest<ErrorOr<List<PostResult>>>;