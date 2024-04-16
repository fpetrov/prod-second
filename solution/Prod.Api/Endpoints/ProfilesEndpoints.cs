using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prod.Api.Common.Extensions;
using Prod.Application.Authentication.Queries;
using Prod.Contracts.Authentication;

namespace Prod.Api.Endpoints;

public static class ProfilesEndpoints
{
    public static RouteGroupBuilder MapProfilesEndpoints(
        this RouteGroupBuilder group)
    {
        group
            .MapGet("/{login}", GetProfile)
            .WithName("GetProfile");
        
        return group;
    }

    private static async Task<IResult> GetProfile(
        [FromRoute] string login,
        HttpContext context,
        [FromServices] ISender sender,
        [FromServices] IMapper mapper)
    {
        var identity = context.GetIdentity();
        
        if (identity is null)
            return Results.Json(new { Reason = "Something went wrong" }, statusCode: 401);
        
        var query = new GetProfileQuery(
            identity.Login,
            login);
        
        var result = await sender.Send(query);
        
        return result.MatchFirst(
            profile => Results.Ok(mapper.Map<GetProfileResponse>(profile)),
            _ => Results.Forbid());
    }
}