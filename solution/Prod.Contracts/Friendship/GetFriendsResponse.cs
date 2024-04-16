namespace Prod.Contracts.Friendship;

public record GetFriendsResponse(
    string Login,
    DateTime AddedAt);
