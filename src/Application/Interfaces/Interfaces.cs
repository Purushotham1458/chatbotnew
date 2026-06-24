using Application.DTOs;

namespace Application.Interfaces;

public interface ITokenService
{
    Task<TokenResponse?> GenerateTokenAsync(TokenRequest request);
}

public interface IChatService
{
    Task<ChatResponse> SendMessageAsync(string clientId, ChatRequest request);
    Task<List<ChatResponse>> GetHistoryAsync(string clientId);
}

public interface IAiProvider
{
    Task<string> GetResponseAsync(string message, string? context = null);
}
