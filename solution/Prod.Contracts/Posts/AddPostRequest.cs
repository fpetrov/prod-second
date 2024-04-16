namespace Prod.Contracts.Posts;

public record AddPostRequest(
    string Content,
    List<string> Tags);