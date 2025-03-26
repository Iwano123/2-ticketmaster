using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Xml.Linq;
using TicketToCode.Api.Endpoints;
using TicketToCode.Api.Services;
using TicketToCode.Core.Data;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddOpenApi();

        // Add CORS services
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowBlazorClient", policy =>
            {
                policy.WithOrigins("https://localhost:7147") // Your Blazor client URL
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials(); // Important if using cookies/auth
            });
        });

        builder.Services.AddSingleton<IDatabase, Database>();
        builder.Services.AddScoped<IAuthService, AuthService>();

        // Add cookie authentication
        builder.Services.AddAuthentication("Cookies")
            .AddCookie("Cookies", options =>
            {
                options.Cookie.Name = "auth";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None; // Changed from Strict for cross-origin requests
            });

        builder.Services.AddAuthorization();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            // Todo: consider scalar? https://youtu.be/Tx49o-5tkis?feature=shared
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "v1");
                options.DefaultModelsExpandDepth(-1);
            });
        }

        app.UseHttpsRedirection();

        // Use CORS - must come before UseAuthentication and UseAuthorization
        app.UseCors("AllowBlazorClient");

        app.UseAuthentication();
        app.UseAuthorization();

        // Map all endpoints
        app.MapEndpoints<Program>();
        app.MapEventEndpoints();
        app.MapBookingEndpoints();
        app.MapUserEndpoints();
        app.MapGet("/secure", [Authorize] () => "This is a secure endpoint!");


        app.Run();
    }
}