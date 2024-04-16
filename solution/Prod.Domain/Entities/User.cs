using System.ComponentModel.DataAnnotations;

namespace Prod.Domain.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string CountryCode { get; set; }
    public bool IsPublic { get; set; }
    public string? Phone { get; set; }
    public string? Image { get; set; }
    public DateTime PasswordChangeDate { get; set; }
    public List<Post> Posts { get; set; } = [];

    public List<Guid> LikedPostsIds { get; set; } = [];
    public List<Guid> DislikedPostsIds { get; set; } = [];
}

/// <summary>
/// User's relationship (just friends, I swear).
/// </summary>
public class Relation
{
    [Key]
    public int Id { get; set; }
    public User User { get; set; }
    public User Friend { get; set; }
    public DateTime StartedAt { get; set; }

    protected Relation() { }

    public static Relation Create(User user, User friend, DateTime startedAt) 
        => new() 
        {
            User = user,
            Friend = friend,
            StartedAt = startedAt
        };
}