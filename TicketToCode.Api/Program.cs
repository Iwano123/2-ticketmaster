using TicketToCode.Api.Endpoints;
using TicketToCode.Api.Services;
using TicketToCode.Core.Data;

var builder = WebApplication.CreateBuilder(args);

// Lägg till Swagger/OpenAPI
builder.Services.AddOpenApi();

//  Lägg till CORS-policy
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

//  Registrera tjänster
builder.Services.AddSingleton<IDatabase, Database>();
builder.Services.AddScoped<IAuthService, AuthService>();

//  Om du använder cookie-autentisering:
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

//  Swagger i Development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
        options.DefaultModelsExpandDepth(-1);
    });
}

//  Https-redirect
app.UseHttpsRedirection();

// Använd CORS innan authentication/authorization
app.UseCors("AllowBlazorClient");

//  Authentication + Authorization
app.UseAuthentication();
app.UseAuthorization();

//  Mappa endpoints
app.MapEndpoints<Program>();

app.MapBookingEndpoints();
app.MapUserEndpoints();


app.MapGet("/secure", [Microsoft.AspNetCore.Authorization.Authorize] () => "Secure endpoint!");

app.Run();
