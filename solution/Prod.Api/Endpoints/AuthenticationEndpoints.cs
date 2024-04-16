using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prod.Application.Authentication.Commands;
using Prod.Contracts.Authentication;

namespace Prod.Api.Endpoints;

public static class AuthenticationEndpoints
{
    public static RouteGroupBuilder MapAuthenticationEndpoints(
        this RouteGroupBuilder group)
    {
        group
            .MapPost("/register", Register);
        
        group.MapPost("/sign-in", Login);
        
        return group;
    }

    private static async Task<IResult> Register(
        [FromBody] RegisterRequest request,
        [FromServices] ISender sender,
        [FromServices] IMapper mapper)
    {
        var command = mapper.Map<RegisterCommand>(request);

        var result = await sender.Send(command);

        return result.MatchFirst(
            authResult 
                => Results.Json(new
                {
                    Profile = mapper.Map<RegisterResponse>(authResult)
                }, statusCode: 201),
            ErrorMatch);
    }
    
    private static async Task<IResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] ISender sender,
        [FromServices] IMapper mapper)
    {
        var command = mapper.Map<LoginCommand>(request);
        
        var result = await sender.Send(command);

        return result.MatchFirst(
            authResult => Results.Ok(mapper.Map<LoginResponse>(authResult)),
            _ => Results.Problem(statusCode: 401, detail: "Неверные логин или пароль."));
    }

    private static IResult ErrorMatch(Error error)
        => error.Type switch
        {
            ErrorType.Validation => Results.BadRequest("Данные переданы в некорректном формате."),
            ErrorType.Conflict => Results.Conflict("Пользователь с таким логином или почтой или телефоном уже существует."),
            _ => Results.NotFound()
        };
}