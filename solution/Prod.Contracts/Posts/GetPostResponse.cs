namespace Prod.Contracts.Posts;

public record GetPostResponse(
    Guid Id,
    string Content,
    List<string> Tags,
    string Author,
    DateTime CreatedAt,
    int LikesCount,
    int DislikesCount);