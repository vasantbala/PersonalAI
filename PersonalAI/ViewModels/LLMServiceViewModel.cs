using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PersonalAI.ViewModels
{
    public class LLMServiceViewModel : INotifyPropertyChanged
    {

        private CollectionView _llmServiceItems;
        private string _selectedLlmService;

        public CollectionView LLMServiceItems { 
            get { return _llmServiceItems; }
            set { _llmServiceItems = value; }
        }

        public string SelectedLLMService
        {
            get { return _selectedLlmService;}
            set 
            {
                if (_selectedLlmService != value)
                {
                    _selectedLlmService = value;
                    OnPropertyChanged(value);
                }

            }
        }

        private void OnPropertyChanged(string llmService)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(llmService));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
