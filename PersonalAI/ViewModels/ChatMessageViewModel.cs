using System.ComponentModel;

namespace PersonalAI.ViewModels
{
    public class ChatMessageViewModel : INotifyPropertyChanged
    {
        public string Role    { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsUser    => Role == "user";

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
