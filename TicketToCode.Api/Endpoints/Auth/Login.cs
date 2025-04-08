using TicketToCode.Api.Services;

namespace TicketToCode.Api.Endpoints.Auth;

public class Login : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) => app
        .MapPost("/auth/login", Handle)
        .WithSummary("Login with username and password")
        .AllowAnonymous();

    public record Request(string Username, string Password);

    private static Results<Ok<UserDto>, NotFound<string>> Handle(
        Request request,
        IAuthService authService,
        HttpContext context)
    {
        var userDto = authService.Login(request.Username, request.Password);
        if (userDto == null)
            return TypedResults.NotFound("Invalid username or password");

        context.Response.Cookies.Append("auth", $"{userDto.Username}:{userDto.Role}", new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });

        return TypedResults.Ok(userDto);
    }
}
