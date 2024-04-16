using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prod.Api.Common.Extensions;
using Prod.Application.Authentication.Commands;
using Prod.Contracts.Authentication;

namespace Prod.Api.Endpoints;

public static class MeEndpoints
{
    public static RouteGroupBuilder MapMeEndpoints(
        this RouteGroupBuilder group)
    {
        group
            .MapGet("/profile", GetMe)
            .WithName("GetMe");

        group
            .MapPatch("/profile", UpdateMe)
            .WithName("UpdateMe");
        
        group
            .MapPost("/updatePassword", UpdatePassword)
            .WithName("UpdatePassword");
        
        return group;
    }

    private static Task<IResult> GetMe(
        HttpContext context,
        [FromServices] IMapper mapper)
    {
        var identity = context.GetIdentity();
        
        return Task.FromResult(
            identity is null
            ? Results.Json(new { Reason = "Something went wrong" }, statusCode: 401)
            : Results.Ok(mapper.Map<GetMeResponse>(identity)));
    }

    private static async Task<IResult> UpdateMe(
        [FromBody] UpdateMeRequest request,
        HttpContext context,
        [FromServices] ISender sender,
        [FromServices] IMapper mapper)
    {
        var identity = context.GetIdentity();
        
        if (identity is null)
            return Results.Json(new { Reason = "Something went wrong" }, statusCode: 401);
        
        var command = mapper.Map<UpdateMeCommand>(request);
        command.Login = identity.Login;

        var result = await sender.Send(command);
        
        return result.MatchFirst(
            response => Results.Ok(mapper.Map<GetMeResponse>(response)),
            ErrorMatch);
    }
    
    private static async Task<IResult> UpdatePassword(
        [FromBody] UpdatePasswordRequest request,
        HttpContext context,
        [FromServices] ISender sender)
    {
        var identity = context.GetIdentity();
        
        if (identity is null)
            return Results.Json(new { Reason = "Something went wrong" }, statusCode: 401);
        
        var command = new UpdatePasswordCommand(
            identity.Login,
            request.OldPassword,
            request.NewPassword);
        
        var result = await sender.Send(command);
        
        return result.MatchFirst(
            _ => Results.Ok(new { status = "ok" }),
            ErrorMatch);
    }

    private static IResult ErrorMatch(Error error)
        => error.Type switch
        {
            ErrorType.Validation => Results.BadRequest("Данные переданы в некорректном формате."),
            ErrorType.Forbidden => Results.Forbid(),
            ErrorType.Conflict => Results.Conflict("Пользователь с таким телефоном уже существует."),
            _ => Results.NotFound()
        };
}