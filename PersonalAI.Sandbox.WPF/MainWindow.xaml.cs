using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PersonalAI.Sandbox.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey(
        [In] IntPtr hWnd,
        [In] int id,
        [In] uint fsModifiers,
        [In] uint vk
        );

        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey(
            [In] IntPtr hWnd,
            [In] int id);

        private HwndSource mSource;
        private const int HOTKEY_ID = 9000;


        public MainWindow()
        {
            InitializeComponent();

            //ResponseStack.Visibility = Visibility.Collapsed;
            ResponseTB.Visibility = Visibility.Collapsed;
            CopyToClipboard.Visibility = Visibility.Collapsed;
            ClearResponse.Visibility = Visibility.Collapsed;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var helper = new WindowInteropHelper(this);
            mSource = HwndSource.FromHwnd(helper.Handle);
            mSource.AddHook(HwndHook);
            RegisterHotKey();
        }

        protected override void OnClosed(EventArgs e)
        {
            mSource.RemoveHook(HwndHook);
            mSource = null;
            UnregisterHotKey();
            base.OnClosed(e);
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
                            OnHotKeyPressed();
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        private void OnHotKeyPressed()
        {
            Show();
            Activate();
            SearchBox.Focus();
        }

        private void RegisterHotKey()
        {
            var helper = new WindowInteropHelper(this);
            const uint VK_S = 0x53;
            const uint MOD_ALT = 0x0001;
            if (!RegisterHotKey(helper.Handle, HOTKEY_ID, MOD_ALT, VK_S))
            {
                MessageBox.Show("Couldn't register hotkey, closing application. Is the app already running?");
                Application.Current.Shutdown();
            }
        }

        private void UnregisterHotKey()
        {
            var helper = new WindowInteropHelper(this);
            UnregisterHotKey(helper.Handle, HOTKEY_ID);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (e.Key == Key.Enter)
                {
                    //ResponseStack.Visibility = Visibility.Visible;
                    ResponseTB.Visibility = Visibility.Visible;
                    CopyToClipboard.Visibility = Visibility.Visible;
                    ClearResponse.Visibility = Visibility.Visible;
                    CopyToClipboard.Focus();
                    //this.Close();
                }

                if(e.Key == Key.Oem5) 
                {
                    ResponseTB.Text = string.Empty;
                }
            }
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void SearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}