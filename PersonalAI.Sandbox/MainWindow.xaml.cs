using Microsoft.UI.Windowing;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Runtime.CompilerServices;
using Windows.System;
using Windows.UI.Popups;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PersonalAI.Sandbox
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private OverlappedPresenter _presenter;
        private VirtualKey _previousVirtualKey = VirtualKey.None;
        public MainWindow()
        {
            this.InitializeComponent();
            this.AppWindow.Resize(new Windows.Graphics.SizeInt32(680, 70));
            ExtendsContentIntoTitleBar = true;
            _presenter = (OverlappedPresenter)AppWindow.Presenter;
            _presenter.IsResizable = false;
            _presenter.IsMaximizable = false;
            _presenter.IsMinimizable = false;
            SearchBox.Width = 670;
            SearchBox.Height = 60;
            SearchBox.MinHeight = 60;
            SearchBox.KeyUp += SearchBox_KeyUp;
            SearchBox.KeyDown += SearchBox_KeyDown;
            
        }

        private void SearchBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
        }

        private void SearchBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (_previousVirtualKey == VirtualKey.Shift && e.Key == VirtualKey.Enter)
            {
                _previousVirtualKey = VirtualKey.None;
                new MessageDialog("Hey you!").ShowAsync().GetAwaiter().GetResult();
            }
            else
            {
                _previousVirtualKey = e.Key;
            }
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            SearchBox.SelectAll();
            //SearchBox.Focus(FocusState.Programmatic);
        }

        //private void myButton_Click(object sender, RoutedEventArgs e)
        //{
        //    myButton.Content = "Clicked";
        //}
    }
}
