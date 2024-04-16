using Microsoft.AspNetCore.Diagnostics;
using Prod.Api;
using Prod.Api.Endpoints;
using Prod.Api.Filters;
using Prod.Api.Middlewares;
using Prod.Application;
using Prod.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register layers.
builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/api/error");

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<AuthorizationMiddleware>();
app.UseMiddleware<GlobalErrorHandlerMiddleware>();

var api = app.MapGroup("/api");

api.MapGet("/ping", () => "ok");

// Global exception handler.
api.MapGet("/error", (HttpContext context) =>
{
    context.Response.StatusCode = StatusCodes.Status400BadRequest;
    
    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    
    return Results.Problem(
        statusCode: StatusCodes.Status400BadRequest,
        title: exception?.Message);
});

api
    .MapGroup("/countries")
    .MapCountriesEndpoints();

api
    .MapGroup("/auth")
    .MapAuthenticationEndpoints();

api
    .MapGroup("/me")
    .MapMeEndpoints()
    .RequireAuthorization();

api
    .MapGroup("/profiles")
    .MapProfilesEndpoints()
    .RequireAuthorization();

api
    .MapGroup("/friends")
    .MapFriendsEndpoints()
    .RequireAuthorization();

api
    .MapGroup("/posts")
    .MapPostsEndpoints()
    .RequireAuthorization();

api.AddEndpointFilter<ErrorReasonFilter>();

// app.Run();

var address = Environment.GetEnvironmentVariable("SERVER_ADDRESS");

app.Run($"http://{address}");

// Stack:
// MediatR
// Mapster
// ErrorOr
// FluentValidation