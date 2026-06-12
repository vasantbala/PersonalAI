using Microsoft.Extensions.Options;
using PersonalAI.Common;

namespace PersonalAI.Core
{
    public class LLMDispatcher : ILLMClient
    {
        private readonly IGradioClient _gradioClient;
        private readonly OpenAIClient _openAIClient;
        private readonly ClaudeClient _claudeClient;
        private readonly AppSettings _appSettings;

        public LLMDispatcher(IGradioClient gradioClient, OpenAIClient openAIClient, ClaudeClient claudeClient, IOptions<AppSettings> appSettings)
        {
            _gradioClient = gradioClient;
            _openAIClient = openAIClient;
            _claudeClient = claudeClient;
            _appSettings = appSettings.Value;
        }

        public Task<string> CompleteAsync(string llmType, IReadOnlyList<ChatMessage> history)
        {
            var config = _appSettings.Providers.FirstOrDefault(p => p.LLMType == llmType);
            return config?.ApiType?.ToLower() switch
            {
                "openai"  => _openAIClient.CompleteAsync(llmType, history),
                "claude"  => _claudeClient.CompleteAsync(llmType, history),
                _         => _gradioClient.Predict(llmType, history.Last().Content)
            };
        }
    }
}
