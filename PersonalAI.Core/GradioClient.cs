using Microsoft.Extensions.Options;
using PersonalAI.Common;
using System.Net.Http;

namespace PersonalAI.Core
{
    public class GradioClient : IGradioClient
    {
        private readonly AppSettings _appSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public GradioClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> Predict(string llmType, string prompt)
        {
            var httpClient = _httpClientFactory.CreateClient(llmType);
            
            if(httpClient.BaseAddress == null) { throw new ArgumentException(nameof(llmType)); }

            var request = new HttpRequestMessage(HttpMethod.Post, "/predict");
            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("prompt", prompt)
            });

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
