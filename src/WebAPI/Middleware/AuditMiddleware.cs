using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Infrastructure.Data;

namespace WebAPI.Middleware;

public class AuditMiddleware
{
    private readonly RequestDelegate _next;

    public AuditMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        var sw = Stopwatch.StartNew();
        var requestBody = await ReadRequestBody(context.Request);

        var originalBody = context.Response.Body;
        using var responseStream = new MemoryStream();
        context.Response.Body = responseStream;

        await _next(context);

        sw.Stop();
        responseStream.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(responseStream).ReadToEndAsync();
        responseStream.Seek(0, SeekOrigin.Begin);
        await responseStream.CopyToAsync(originalBody);

        var clientId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        db.ApiAuditLogs.Add(new ApiAuditLog
        {
            ClientId = clientId,
            HttpMethod = context.Request.Method,
            Endpoint = context.Request.Path,
            StatusCode = context.Response.StatusCode,
            RequestBody = requestBody.Length > 2000 ? requestBody[..2000] : requestBody,
            ResponseBody = responseBody.Length > 2000 ? responseBody[..2000] : responseBody,
            ElapsedMs = sw.ElapsedMilliseconds
        });
        await db.SaveChangesAsync();
    }

    private static async Task<string> ReadRequestBody(HttpRequest request)
    {
        request.EnableBuffering();
        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;
        return body;
    }
}
