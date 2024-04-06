using Microsoft.Extensions.Options;
using PersonalAI.Common;

namespace PersonalAI.Core
{
    public class GradioClient : IDisposable, IGradioClient
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;
        public GradioClient(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _httpClient = new HttpClient();
            if(_appSettings?.GradleSettings?.Length == 0) 
            {
                throw new ArgumentException("No GradleSettings found.");
            }
            _httpClient.BaseAddress = new Uri(_appSettings.GradleSettings[0].Url);
        }

        public async Task<string> Predict(string prompt)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/predict");
            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("prompt", prompt)
            });

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
