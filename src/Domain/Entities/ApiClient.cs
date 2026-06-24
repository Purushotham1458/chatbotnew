namespace Domain.Entities;

public class ApiClient
{
    public int Id { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
