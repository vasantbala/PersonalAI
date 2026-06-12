using Microsoft.Extensions.Options;
using PersonalAI.Common;
using System.Text;
using System.Text.Json;

namespace PersonalAI.Core
{
    public class ClaudeClient : ILLMClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppSettings _appSettings;

        public ClaudeClient(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings)
        {
            _httpClientFactory = httpClientFactory;
            _appSettings = appSettings.Value;
        }

        public async Task<string> CompleteAsync(string llmType, IReadOnlyList<ChatMessage> history)
        {
            var config = _appSettings.Providers.First(p => p.LLMType == llmType);
            var apiKey = ApiKeyResolver.Resolve(config.ApiKey);

            var httpClient = _httpClientFactory.CreateClient(llmType);

            var request = new HttpRequestMessage(HttpMethod.Post, "v1/messages");
            if (!string.IsNullOrEmpty(apiKey))
                request.Headers.Add("x-api-key", apiKey);
            request.Headers.Add("anthropic-version", "2023-06-01");

            var messages = history.Select(m => new { role = m.Role, content = m.Content });
            var body = new { model = config.ModelName, max_tokens = 4096, messages };
            request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseBody);
            return doc.RootElement
                .GetProperty("content")[0]
                .GetProperty("text")
                .GetString() ?? string.Empty;
        }
    }
}
