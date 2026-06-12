using Microsoft.Extensions.Options;
using PersonalAI.Common;
using System.Text;
using System.Text.Json;

namespace PersonalAI.Core
{
    public class OpenAIClient : ILLMClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppSettings _appSettings;

        public OpenAIClient(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings)
        {
            _httpClientFactory = httpClientFactory;
            _appSettings = appSettings.Value;
        }

        public async Task<string> CompleteAsync(string llmType, IReadOnlyList<ChatMessage> history)
        {
            var config = _appSettings.Providers.First(p => p.LLMType == llmType);
            var apiKey = ApiKeyResolver.Resolve(config.ApiKey);

            var httpClient = _httpClientFactory.CreateClient(llmType);

            var request = new HttpRequestMessage(HttpMethod.Post, "v1/chat/completions");
            if (!string.IsNullOrEmpty(apiKey))
                request.Headers.Add("Authorization", $"Bearer {apiKey}");

            var messages = history.Select(m => new { role = m.Role, content = m.Content });
            var body = new { model = config.ModelName, messages };
            request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseBody);
            return doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? string.Empty;
        }
    }
}
