namespace Application.DTOs;

public record TokenRequest(string ClientId, string Secret);
public record TokenResponse(string Token);
public record ChatRequest(string Message);
public record ChatResponse(string Reply, DateTime Timestamp);
