using TicketToCode.Core.Data;
using TicketToCode.Core.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TicketToCode.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/users/{id}", (int id, IDatabase db) =>
        {
            var user = db.GetUser(id);
            return user != null ? Results.Ok(user) : Results.NotFound();
        }).WithSummary("Get user by ID");

        app.MapPut("/users/{id}", (int id, User updatedUser, IDatabase db) =>
        {
            var user = db.GetUser(id);
            if (user == null) return Results.NotFound();

            db.UpdateUser(updatedUser);
            return Results.NoContent();
        }).WithSummary("Update a user");

        app.MapDelete("/users/{id}", (int id, IDatabase db) =>
        {
            var user = db.GetUser(id);
            if (user == null) return Results.NotFound();

            db.DeleteUser(id);
            return Results.NoContent();
        }).WithSummary("Delete a user");
    }
}