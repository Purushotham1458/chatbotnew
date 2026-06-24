namespace Domain.Entities;

public class ErrorLog
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
    public string? Endpoint { get; set; }
    public string? ClientId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
