namespace Prod.Contracts.Posts;

public record AddPostResponse(
    Guid Id,
    string Content,
    List<string> Tags,
    string Author,
    DateTime CreatedAt,
    int LikesCount,
    int DislikesCount);