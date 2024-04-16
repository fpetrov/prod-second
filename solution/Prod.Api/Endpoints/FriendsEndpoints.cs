using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prod.Api.Common.Extensions;
using Prod.Application.Friendship.Commands;
using Prod.Application.Friendship.Queries;
using Prod.Contracts.Friendship;
using Prod.Domain.Common.Extentions;

namespace Prod.Api.Endpoints;

public static class FriendsEndpoints
{
    public static RouteGroupBuilder MapFriendsEndpoints(
        this RouteGroupBuilder group)
    {
        group.MapGet("/", GetFriends);
        group.MapPost("/add", AddFriend);
        group.MapPost("/remove", RemoveFriend);

        return group;
    }

    private static async Task<IResult> RemoveFriend(
        [FromBody] RemoveFriendRequest request,
        HttpContext context,
        [FromServices] ISender sender)
    {
        var identity = context.GetIdentity();
        
        if (identity is null)
            return Results.Json(new { Reason = "Something went wrong" }, statusCode: 401);
        
        var command = new RemoveFriendCommand(
            identity.Login,
            request.Login);
        
        await sender.Send(command);

        return Results.Ok(new { status = "ok" });
    }
    
    private static async Task<IResult> GetFriends(
        HttpContext context,
        [FromServices] ISender sender,
        [FromQuery] int? limit = 5,
        [FromQuery] int? offset = 0)
    {
        var identity = context.GetIdentity();
        
        if (identity is null)
            return Results.Json(new { Reason = "Something went wrong" }, statusCode: 401);
        
        var query = new GetFriendsQuery(
            identity.Login,
            limit!.Value,
            offset!.Value);
        
        var friends = await sender
            .Send(query);

        if (friends.Count == 0)
            return Results.Ok(Array.Empty<int>());
        
        // TODO: Настроить правильный формат вывода даты.
        return Results
            .Json(friends
                .ConvertAll(t =>
                    new GetFriendsResponse(t.Friend.Login, t.StartedAt)),
                statusCode: 200,
                options: DateTimeExtensions.SerializerOptions);
    }
    
    private static async Task<IResult> AddFriend(
        [FromBody] AddFriendRequest request,
        HttpContext context,
        [FromServices] ISender sender)
    {
        var identity = context.GetIdentity();
        
        if (identity is null)
            return Results.Json(new { Reason = "Something went wrong" }, statusCode: 401);
        
        var command = new AddFriendCommand(
            identity.Login,
            request.Login);
        
        var result = await sender.Send(command);
        
        return result.Match(
            _ => Results.Ok(new { status = "ok" }),
            _ => Results.NotFound());
    }
}