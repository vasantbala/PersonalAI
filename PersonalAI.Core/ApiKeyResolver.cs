namespace PersonalAI.Core
{
    public static class ApiKeyResolver
    {
        public static string Resolve(string? apiKey)
        {
            if (string.IsNullOrEmpty(apiKey)) return string.Empty;
            if (apiKey.StartsWith("${") && apiKey.EndsWith("}"))
                return Environment.GetEnvironmentVariable(apiKey[2..^1]) ?? string.Empty;
            return apiKey;
        }
    }
}
