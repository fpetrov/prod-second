using MediatR;
using Prod.Application.Common.Interfaces.Persistence;
using Prod.Application.Posts.Common;

namespace Prod.Application.Posts.Queries;

public class GetMyPostsFeedQueryHandler(
    IPostRepository postRepository)
    : IRequestHandler<GetMyPostsFeedQuery, List<PostResult>>
{
    public async Task<List<PostResult>> Handle(
        GetMyPostsFeedQuery request,
        CancellationToken cancellationToken)
    {
        var (login, limit, offset) = request;

        var posts = await postRepository.GetByAuthor(
            login,
            limit,
            offset,
            cancellationToken);
        
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

public record GetMyPostsFeedQuery(
    string Login,
    int Limit,
    int Offset)
    : IRequest<List<PostResult>>;