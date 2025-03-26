using TicketToCode.Core.Data;
using TicketToCode.Core.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;

namespace TicketToCode.Api.Endpoints;

public static class EventEndpoints
{
    public static void MapEventEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPut("/events/{id}", (int id, Event updatedEvent, IDatabase db) =>
        {
            var evt = db.GetEvent(id);
            if (evt == null) return Results.NotFound();

            db.UpdateEvent(updatedEvent);
            return Results.NoContent();
        }).WithSummary("Update an event");

        app.MapDelete("/events/{id}", (int id, IDatabase db) =>
        {
            var evt = db.GetEvent(id);
            if (evt == null) return Results.NotFound();

            db.DeleteEvent(id);
            return Results.NoContent();
        }).WithSummary("Delete an event");
    }
}