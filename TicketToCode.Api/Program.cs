using TicketToCode.Api.Endpoints;
using TicketToCode.Api.Services;
using TicketToCode.Core.Data;

var builder = WebApplication.CreateBuilder(args);

// L�gg till Swagger/OpenAPI
builder.Services.AddOpenApi();

//  L�gg till CORS-policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:7057") // <-- Exakt adress d�r Blazor k�r
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
        // Endast om du anv�nder cookies/credentials. 
        // Om du INTE anv�nder cookies kan du hoppa �ver .AllowCredentials().
    });
});

//  Registrera tj�nster
builder.Services.AddSingleton<IDatabase, Database>();
builder.Services.AddScoped<IAuthService, AuthService>();

//  Om du anv�nder cookie-autentisering:
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.Cookie.Name = "auth";
        options.Cookie.HttpOnly = true;
        // Vid cross-origin med cookies kr�vs SameSite=None
        options.Cookie.SameSite = SameSiteMode.None;
        // Om du vill att cookies endast ska skickas �ver https
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

// Anv�nd CORS innan authentication/authorization
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
