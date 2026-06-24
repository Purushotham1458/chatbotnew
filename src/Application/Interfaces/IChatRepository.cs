using Application.DTOs;

namespace Application.Interfaces;

public interface IChatRepository
{
    Task SaveMessageAsync(string clientId, string userMessage, string botResponse);
    Task<List<ChatResponse>> GetHistoryAsync(string clientId);
}
