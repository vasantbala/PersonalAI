namespace PersonalAI.Common
{
    public class LLMProviderConfig
    {
        public string LLMType   { get; set; }
        public string Url       { get; set; }
        public string ApiType   { get; set; }  // "openai" | "claude" | "gradio"
        public string ApiKey    { get; set; }  // literal value OR "${ENV_VAR_NAME}"
        public string ModelName { get; set; }
    }
}
