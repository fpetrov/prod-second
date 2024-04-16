using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prod.Application.Countries.Queries;

namespace Prod.Api.Endpoints;

public static class CountriesEndpoints
{
    public static RouteGroupBuilder MapCountriesEndpoints(
        this RouteGroupBuilder group)
    {
        group.MapGet("/", FilterCountries);
        group.MapGet("/{alpha2}", GetCountryByAlpha2);
        
        return group;
    }

    // TODO: Maybe cast requst to custom type (but I'm too lazy)
    private static async Task<IResult> FilterCountries(
        [FromQuery(Name = "region")] string[]? request,
        [FromServices] ISender sender)
    {
        if (request is null || request.Length == 0)
            request = ["Europe", "Africa", "Americas", "Oceania", "Asia"];
        
        var command = new FilterCountriesQuery(request.ToList());

        var result = await sender.Send(command);
        
        return result.Match(
            Results.Ok,
            _ => Results.BadRequest("Ошибка в запросе"));
    }

    private static async Task<IResult> GetCountryByAlpha2(
        [FromRoute] string alpha2,
        [FromServices] ISender sender)
    {
        var command = new GetCountryByCodeQuery(alpha2);
        
        var result = await sender.Send(command);

        return result
            .Match(
                Results.Ok,
                _ => Results.NotFound("Страна не найдена"));
    }
}