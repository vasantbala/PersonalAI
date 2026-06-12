namespace PersonalAI.Core
{
    public interface ILLMClient
    {
        Task<string> CompleteAsync(string llmType, IReadOnlyList<ChatMessage> history);
    }
}
