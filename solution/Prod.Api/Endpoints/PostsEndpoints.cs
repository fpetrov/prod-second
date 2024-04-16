using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prod.Api.Common.Extensions;
using Prod.Application.Posts.Commands;
using Prod.Application.Posts.Queries;
using Prod.Contracts.Posts;
using Prod.Domain.Common.Extentions;

namespace Prod.Api.Endpoints;

public static class PostsEndpoints
{
    public static RouteGroupBuilder MapPostsEndpoints(
        this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", GetPost);
        group.MapGet("/feed/my", GetMyPostsFeed);
        group.MapGet("/feed/{login}", GetPostsFeed);
        group.MapPost("/new", AddPost);
        group.MapPost("/{postId:guid}/like", LikePost);
        group.MapPost("/{postId:guid}/dislike", DislikePost);

        return group;
    }

    private static async Task<IResult> LikePost(
        [FromRoute] Guid postId,
        HttpContext context,
        [FromServices] ISender sender)
    {
        var identity = context.GetIdentity();
        
        if (identity is null)
            return Results.Json(new { Reason = "Something went wrong" }, statusCode: 401);
        
        var command = new LikePostCommand(
            identity.Login,
            postId);
        
        var result = await sender
            .Send(command);
        
        return result.MatchFirst(
            postResult => Results.Json(postResult, statusCode: 200, options: DateTimeExtensions.SerializerOptions),
            _ => Results.NotFound());
    }
    
    private static async Task<IResult> DislikePost(
        [FromRoute] Guid postId,
        HttpContext context,
        [FromServices] ISender sender)
    {
        var identity = context.GetIdentity();
        
        if (identity is null)
            return Results.Json(new { Reason = "Something went wrong" }, statusCode: 401);
        
        var command = new DislikePostCommand(
            identity.Login,
            postId);
        
        var result = await sender
            .Send(command);
        
        return result.MatchFirst(
            postResult => Results.Json(postResult, statusCode: 200, options: DateTimeExtensions.SerializerOptions),
            _ => Results.NotFound());
    }
    
    private static async Task<IResult> GetPostsFeed(
        [FromRoute] string login,
        HttpContext context,
        [FromServices] ISender sender,
        [FromServices] IMapper mapper,
        [FromQuery] int? limit = 5,
        [FromQuery] int? offset = 0)
    {
        var identity = context.GetIdentity();

        if (identity is null)
            return Results.Json(new { Reason = "Something went wrong" }, statusCode: 401);

        var query = new GetPostsFeedQuery(
            identity.Login,
            login,
            limit!.Value,
            offset!.Value);

        var posts = await sender
            .Send(query);

        return posts.MatchFirst(
            postResults => Results.Json(
                postResults,
                statusCode: 200,
                options: DateTimeExtensions.SerializerOptions),
            _ => Results.NotFound());
    }

    private static async Task<IResult> GetMyPostsFeed(
        HttpContext context,
        [FromServices] ISender sender,
        [FromServices] IMapper mapper,
        [FromQuery] int? limit = 5,
        [FromQuery] int? offset = 0)
    {
        var identity = context.GetIdentity();

        if (identity is null)
            return Results.Json(new { Reason = "Something went wrong" }, statusCode: 401);

        var query = new GetMyPostsFeedQuery(
            identity.Login,
            limit!.Value,
            offset!.Value);

        var posts = await sender
            .Send(query);

        if (posts.Count == 0)
            return Results.Ok(Array.Empty<int>());

        return Results.Json(posts
            .ConvertAll(t => new GetPostsFeedResponse(
                t.Id,
                t.Content,
                t.Tags,
                t.Author,
                t.CreatedAt,
                t.LikesCount,
                t.DislikesCount)),
            statusCode: 200,
            options: DateTimeExtensions.SerializerOptions);
    }

    private static async Task<IResult> GetPost(
        [FromRoute] Guid id,
        HttpContext context,
        [FromServices] ISender sender,
        [FromServices] IMapper mapper)
    {
        var identity = context.GetIdentity();
        
        if (identity is null)
            return Results.Json(new { Reason = "Something went wrong" }, statusCode: 401);
        
        var query = new GetPostQuery(
            identity.Login,
            id);
        
        var result = await sender.Send(query);

        return result.MatchFirst(
            postResult => Results.Json(mapper.Map<GetPostResponse>(postResult),
                statusCode: 200,
                options: DateTimeExtensions.SerializerOptions),
            _ => Results.NotFound());
    }

    private static async Task<IResult> AddPost(
        [FromBody] AddPostRequest request,
        HttpContext context,
        [FromServices] ISender sender,
        [FromServices] IMapper mapper)
    {
        var identity = context.GetIdentity();

        if (identity is null)
            return Results.Json(new { Reason = "Something went wrong" }, statusCode: 401);

        var command = new AddPostCommand(
            identity.Login,
            request.Content,
            request.Tags);

        var result = await sender.Send(command);

        return Results.Json(
            mapper.Map<AddPostResponse>(result),
            statusCode: 200,
            options: DateTimeExtensions.SerializerOptions);
    }
}