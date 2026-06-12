namespace PersonalAI.Core
{
    public class ChatMessage
    {
        public string Role    { get; set; }  // "user" | "assistant"
        public string Content { get; set; }
    }
}
