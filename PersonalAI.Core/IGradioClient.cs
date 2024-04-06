
namespace PersonalAI.Core
{
    public interface IGradioClient
    {
        void Dispose();
        Task<string> Predict(string prompt);
    }
}