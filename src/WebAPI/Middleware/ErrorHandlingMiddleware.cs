using System.Security.Claims;
using System.Text.Json;
using Domain.Entities;
using Infrastructure.Data;

namespace WebAPI.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] {context.Request.Method} {context.Request.Path}: {ex}");

            try
            {
                var clientId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                db.ErrorLogs.Add(new ErrorLog
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Endpoint = $"{context.Request.Method} {context.Request.Path}",
                    ClientId = clientId
                });
                await db.SaveChangesAsync();
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"[ERROR] Failed to log error: {logEx.Message}");
            }

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "An internal error occurred." }));
        }
    }
}
