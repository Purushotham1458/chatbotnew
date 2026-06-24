using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService) => _tokenService = tokenService;

    [HttpPost]
    public async Task<IActionResult> GenerateToken(TokenRequest request)
    {
        var result = await _tokenService.GenerateTokenAsync(request);
        if (result == null)
            return Unauthorized(new { error = "Invalid client credentials" });
        return Ok(result);
    }
}
