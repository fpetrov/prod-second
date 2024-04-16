using System.ComponentModel.DataAnnotations;

namespace Prod.Domain.Entities;

public class Post
{
    [Key] public int Id { get; set; }
    public Guid Uuid { get; set; }
    public string Content { get; set; }
    public List<string> Tags { get; set; } = [];
    public User Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public int LikesCount { get; set; }
    public int DislikesCount { get; set; }

    protected Post() { }

    public static Post Create(
        Guid uuid,
        string content,
        List<string> tags,
        User author,
        DateTime createdAt)
        => new()
        {
            Uuid = uuid,
            Content = content,
            Tags = tags,
            Author = author,
            CreatedAt = createdAt
        };
}