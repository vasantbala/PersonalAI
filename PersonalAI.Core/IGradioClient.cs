
namespace PersonalAI.Core
{
    public interface IGradioClient
    {
        Task<string> Predict(string llmType, string prompt);
    }
}