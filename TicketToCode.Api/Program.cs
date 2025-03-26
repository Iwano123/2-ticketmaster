using TicketToCode.Api.Endpoints;
using TicketToCode.Api.Services;
using TicketToCode.Core.Data;

var builder = WebApplication.CreateBuilder(args);

// 1) Lägg till Swagger/OpenAPI
builder.Services.AddOpenApi();

// 2) Lägg till CORS-policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:7057") // <-- Exakt adress där Blazor kör
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
        // Endast om du använder cookies/credentials. 
        // Om du INTE använder cookies kan du hoppa över .AllowCredentials().
    });
});

// 3) Registrera tjänster
builder.Services.AddSingleton<IDatabase, Database>();
builder.Services.AddScoped<IAuthService, AuthService>();

// 4) Om du använder cookie-autentisering:
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.Cookie.Name = "auth";
        options.Cookie.HttpOnly = true;
        // Vid cross-origin med cookies krävs SameSite=None
        options.Cookie.SameSite = SameSiteMode.None;
        // Om du vill att cookies endast ska skickas över https
        // options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// 5) Swagger i Development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
        options.DefaultModelsExpandDepth(-1);
    });
}

// 6) Https-redirect
app.UseHttpsRedirection();

// 7) Använd CORS innan authentication/authorization
app.UseCors("AllowBlazorClient");

// 8) Authentication + Authorization
app.UseAuthentication();
app.UseAuthorization();

// 9) Mappa endpoints
app.MapEndpoints<Program>();
app.MapEventEndpoints();
app.MapBookingEndpoints();
app.MapUserEndpoints();

// Exempel på en skyddad endpoint
app.MapGet("/secure", [Microsoft.AspNetCore.Authorization.Authorize] () => "Secure endpoint!");

app.Run();
