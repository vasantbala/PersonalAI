using PersonalAI.Core;
using System.Windows;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using PersonalAI.ViewModels;
using Microsoft.Extensions.Options;
using PersonalAI.Common;

namespace PersonalAI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        // Import the user32.dll
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // Define the hotkey id
        private const int HOTKEY_ID = 9000;

        // Define the modifier keys
        private const uint MOD_NONE = 0x0000; //(none)
        private const uint MOD_ALT = 0x0001; //ALT
        private const uint MOD_CONTROL = 0x0002; //CTRL
        private const uint MOD_SHIFT = 0x0004; //SHIFT
        private const uint MOD_WIN = 0x0008; //WINDOWS

        private LLMServiceViewModel _llmServiceList = new();

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Register the hotkey
            bool result = RegisterHotKey(new WindowInteropHelper(this).Handle, HOTKEY_ID, MOD_CONTROL, (uint)KeyInterop.VirtualKeyFromKey(Key.F1));

            if (!result)
            {
                System.Windows.MessageBox.Show("Another instance is already running. Will exit now.");
                Close();
            }
        }

        private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            // Unregister the hotkey
            UnregisterHotKey(new WindowInteropHelper(this).Handle, HOTKEY_ID);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(HwndHook);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            int vkey = (((int)lParam >> 16) & 0xFFFF);
                            if (vkey == KeyInterop.VirtualKeyFromKey(Key.F1))
                            {
                                this.WindowState = WindowState.Normal;
                            }
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        
        private readonly IGradioClient _gradioClient;
        private readonly AppSettings _appSettings;

        public MainWindow(IGradioClient gradioClient, IOptions<AppSettings> appSettings)
        {
            InitializeComponent();
            
            Loaded += MainWindow_Loaded;
            Unloaded += MainWindow_Unloaded;
            _gradioClient = gradioClient;
            _appSettings = appSettings.Value;
            
            PopulateLLMTypeList();
            DataContext = _llmServiceList;

            ToggleSettingsPanel();
            CollapseResponse();
        }

        private void PopulateLLMTypeList()
        {
            var list = new List<LLMServiceItemViewModel>();
            list.Add(new LLMServiceItemViewModel() { Name = "Choose a model", BaseUrl = "" });
            if (_appSettings?.GradleSettings?.Length > 0)
            {
                foreach (var item in _appSettings.GradleSettings)
                {
                    list.Add(new LLMServiceItemViewModel()
                    {
                        BaseUrl = item.Url,
                        Name = item.LLMType
                    });
                }
                _llmServiceList.LLMServiceItems = new System.Windows.Data.CollectionView(list);
                _llmServiceList.SelectedLLMService = list[1].Name;
            }
        }


        private async void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            await Dispatcher.InvokeAsync(async () =>
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    if (e.Key == Key.Enter)
                    {
                        if (LLMOptions.SelectedIndex == 0)
                        {
                            System.Windows.MessageBox.Show("Choose a valid LLM Model to continue.");
                            SettingBtn.IsChecked = true;
                            ToggleSettingsPanel();
                            return;
                        }

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
                ResponseTB.Text = await _gradioClient.Predict((string)LLMOptions.SelectedValue, SearchBox.Text);
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
            WindowState = WindowState.Minimized;
        }

        private void ClearResponseBtn_Click(object sender, RoutedEventArgs e)
        {
            CollapseResponse(true);

        }

        private void CopyToClipboardBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText(ResponseTB.Text);
        }

        private void SettingBtn_Click(object sender, RoutedEventArgs e)
        {
            ToggleSettingsPanel();
        }

        private void ToggleSettingsPanel()
        {
            if (SettingBtn.IsChecked.HasValue && SettingBtn.IsChecked.Value)
            {
                SettingsPanel.Visibility = Visibility.Visible;
            }
            else
            {
                SettingsPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void headerThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Left = Left + e.HorizontalChange;
            Top = Top + e.VerticalChange;
        }
    }
}