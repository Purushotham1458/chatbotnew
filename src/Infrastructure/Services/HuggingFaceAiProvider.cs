using System.Net.Http.Json;
using System.Text.Json;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class HuggingFaceAiProvider : IAiProvider
{
    private readonly HttpClient _http;
    private readonly string _apiKey;
    private readonly string _model;

    public HuggingFaceAiProvider(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["HuggingFace:ApiKey"]!;
        _model = config["HuggingFace:Model"] ?? "meta-llama/Llama-3.1-8B-Instruct";
    }

    public async Task<string> GetResponseAsync(string message, string? context = null)
    {
        var url = "https://router.huggingface.co/v1/chat/completions";

        var messages = new List<object>();

        var systemPrompt = "You are a helpful loan assistant chatbot. When loan data is provided, explain it clearly in a structured and user-friendly way using markdown formatting. Be concise but informative.";

        if (!string.IsNullOrEmpty(context))
        {
            systemPrompt += "\n\n" + context;
        }

        messages.Add(new { role = "system", content = systemPrompt });
        messages.Add(new { role = "user", content = message });

        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("Authorization", $"Bearer {_apiKey}");
        request.Content = JsonContent.Create(new
        {
            model = _model,
            messages,
            max_tokens = 1000,
            temperature = 0.7
        });

        HttpResponseMessage response;
        try
        {
            response = await _http.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            return $"AI service unavailable: {ex.Message}";
        }

        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return $"AI service error: {json}";

        var result = JsonSerializer.Deserialize<JsonElement>(json);
        if (result.TryGetProperty("choices", out var choices) &&
            choices.GetArrayLength() > 0)
        {
            return choices[0].GetProperty("message").GetProperty("content").GetString()?.Trim()
                   ?? "No response from AI";
        }

        return "No response from AI";
    }
}
