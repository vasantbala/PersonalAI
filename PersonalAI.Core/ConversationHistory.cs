namespace PersonalAI.Core
{
    public class ConversationHistory
    {
        private readonly List<ChatMessage> _messages = new();

        public void Add(string role, string content) =>
            _messages.Add(new ChatMessage { Role = role, Content = content });

        public void Clear() => _messages.Clear();

        // Indirection point: replace with summarization/compaction logic here later
        // without changing ILLMClient or any callers.
        public IReadOnlyList<ChatMessage> GetEffectiveHistory() => _messages.AsReadOnly();
    }
}
