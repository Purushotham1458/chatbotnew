namespace Domain.Entities;

public class ChatMessage
{
    public int Id { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public string UserMessage { get; set; } = string.Empty;
    public string BotResponse { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
