using PersonalAI.Core;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace PersonalAI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IGradioClient _gradioClient;
        //public MainWindow()
        //{ 
        //}

        public MainWindow(IGradioClient gradioClient)
        {
            InitializeComponent();
            _gradioClient = gradioClient;
            CollapseResponse();
        }

        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            await Dispatcher.InvokeAsync(async () =>
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    if (e.Key == Key.Enter)
                    {
                        CollapseResponse();
                        ResponseLoading.Visibility = Visibility.Visible;
                        await DoPredict();
                        ResponseLoading.Visibility = Visibility.Collapsed;
                        ShowResponse();
                    }

                    if (e.Key == Key.Oem5)
                    {
                        ResponseTB.Text = string.Empty;
                    }
                }
                
            });
            
        }

        private async Task DoPredict()
        {
            try
            {
                ResponseTB.Text = await _gradioClient.Predict(SearchBox.Text);
            }
            catch (Exception ex) 
            {
                ResponseTB.Text = "Something went wrong. Try again later...";
            }
        }

        private void ShowResponse()
        {
            ResponseTB.Visibility = Visibility.Visible;
            CopyToClipboardBtn.Visibility = Visibility.Visible;
            ClearResponseBtn.Visibility = Visibility.Visible;
            CopyToClipboardBtn.Focus();
        }

        private void CollapseResponse(bool clearContent=true)
        {
            ResponseTB.Visibility = Visibility.Collapsed;
            CopyToClipboardBtn.Visibility = Visibility.Collapsed;
            ClearResponseBtn.Visibility = Visibility.Collapsed;
            if (clearContent) 
            {
                ResponseTB.Text = string.Empty;
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ClearResponseBtn_Click(object sender, RoutedEventArgs e)
        {
            ResponseTB.Text = string.Empty;
        }

        private void CopyToClipboardBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText(ResponseTB.Text);
        }
    }
}