using System.Security.Claims;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IChatService _chat;

    public ChatController(IChatService chat) => _chat = chat;

    [HttpPost]
    public async Task<IActionResult> Send(ChatRequest request)
    {
        var clientId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _chat.SendMessageAsync(clientId, request);
        return Ok(result);
    }

    [HttpGet("history")]
    public async Task<IActionResult> History()
    {
        var clientId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _chat.GetHistoryAsync(clientId);
        return Ok(result);
    }
}
