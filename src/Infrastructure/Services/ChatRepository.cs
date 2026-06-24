using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ChatRepository : IChatRepository
{
    private readonly AppDbContext _db;

    public ChatRepository(AppDbContext db) => _db = db;

    public async Task SaveMessageAsync(string clientId, string userMessage, string botResponse)
    {
        _db.ChatMessages.Add(new ChatMessage
        {
            ClientId = clientId,
            UserMessage = userMessage,
            BotResponse = botResponse
        });
        await _db.SaveChangesAsync();
    }

    public async Task<List<ChatResponse>> GetHistoryAsync(string clientId)
    {
        return await _db.ChatMessages
            .Where(c => c.ClientId == clientId)
            .OrderByDescending(c => c.CreatedAt)
            .Take(50)
            .Select(c => new ChatResponse(c.BotResponse, c.CreatedAt))
            .ToListAsync();
    }
}
