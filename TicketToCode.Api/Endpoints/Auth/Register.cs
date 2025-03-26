using TicketToCode.Api.Services;

namespace TicketToCode.Api.Endpoints.Auth;

public class Register : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) => app
        .MapPost("/auth/register", Handle)
        .WithSummary("Register a new user")
        .AllowAnonymous();

    public record Request(string Username, string Password);

    private static Results<Ok<UserDto>, BadRequest<string>> Handle(
        Request request,
        IAuthService authService)
    {
        var userDto = authService.Register(request.Username, request.Password);
        if (userDto == null)
            return TypedResults.BadRequest("Username already exists");

        return TypedResults.Ok(userDto);
    }
}
