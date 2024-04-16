namespace Prod.Application.Posts.Common;

public record PostResult(
    Guid Id,
    string Content,
    List<string> Tags,
    string Author,
    DateTime CreatedAt,
    int LikesCount,
    int DislikesCount);