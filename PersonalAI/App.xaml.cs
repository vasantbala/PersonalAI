using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonalAI.Common;
using PersonalAI.Core;
using System.IO;
using System.Windows;
using NotifyIcon = System.Windows.Forms.NotifyIcon;

namespace PersonalAI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        private MainWindow _mainWindow = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            NotifyIcon icon = new NotifyIcon() 
            {
                Icon = ProjectResources.PersonalAI,
                Visible = true
            };
            icon.Click += Icon_Click;
            

            // Create a context menu with an "Exit" item
            var contextMenuStrip = new ContextMenuStrip();
            var exitMenu = new System.Windows.Forms.ToolStripMenuItem("Exit");
            exitMenu.Click += ExitMenu_Click;
            contextMenuStrip.Items.Add(exitMenu);

            // Assign the context menu to the NotifyIcon
            icon.ContextMenuStrip = contextMenuStrip;

            ShowMainWindow();
        }

        private void ExitMenu_Click(object? sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Icon_Click(object? sender, EventArgs e)
        {
            ShowMainWindow();
        }

        private void ShowMainWindow()
        {
            if (_mainWindow == null)
            {
                _mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            }
            _mainWindow.WindowState = WindowState.Normal;
            _mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));
            //var appSettings = Configuration.Get<AppSettings>();
            services.AddScoped<IGradioClient, GradioClient>();
            //services.AddTransient(typeof(MainWindow)), factory => new MainWindow();
            services.AddTransient(typeof(MainWindow));
        }
    }

}
