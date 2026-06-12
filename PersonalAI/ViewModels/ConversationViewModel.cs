using PersonalAI.Core;
using System.Collections.ObjectModel;

namespace PersonalAI.ViewModels
{
    public class ConversationViewModel
    {
        private readonly ConversationHistory _history = new();

        public ObservableCollection<ChatMessageViewModel> Messages { get; } = new();

        public void AddMessage(string role, string content)
        {
            _history.Add(role, content);
            Messages.Add(new ChatMessageViewModel { Role = role, Content = content });
        }

        public void Clear()
        {
            _history.Clear();
            Messages.Clear();
        }

        public IReadOnlyList<ChatMessage> GetEffectiveHistory() => _history.GetEffectiveHistory();
    }
}
