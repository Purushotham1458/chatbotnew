namespace Domain.Entities;

public class ApiAuditLog
{
    public int Id { get; set; }
    public string? ClientId { get; set; }
    public string HttpMethod { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string? RequestBody { get; set; }
    public string? ResponseBody { get; set; }
    public long ElapsedMs { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
