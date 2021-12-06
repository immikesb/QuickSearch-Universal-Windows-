using Hardcodet.Wpf.TaskbarNotification;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows;
using System.Data;
using System.Linq;
using System;
using System.IO;
using System.Windows.Input;
using QuickSearch.Services;

namespace QuickSearch
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when the System.Windows.Application.Run method of the System.Windows.Application
        /// object is called.
        /// </summary>
        /// <param name="e">A System.Windows.StartupEventArgs that contains the event data.</param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            Stream iconStream = GetResourceStream(new Uri("/Assets/AppIcon.ico", UriKind.Relative)).Stream;
            trayIcon = new()
            {
                ContextMenu = Resources["TrayIconContextMenu"] as ContextMenu,
                Icon = new System.Drawing.Icon(iconStream),
                ToolTipText = "QuickSearch is running"
            };
            //notifyIcon.ContextMenu = 
            await iconStream.DisposeAsync();

            keyHook = new();
            // register the event that is fired after the key press.
            keyHook.KeyPressed += OnKeyPressed;
            // register the ctrl + alt + Space combination as hot key.
            keyHook.RegisterHotKey(ModifierKeys.Alt, System.Windows.Forms.Keys.Space);

            m_window = new MainWindow();
        }

        /// <summary>
        /// Occurs just before an application shuts down and cannot be canceled.
        /// </summary>
        /// <param name="e">An System.Windows.ExitEventArgs that contains the event data.</param>
        protected override void OnExit(ExitEventArgs e)
        {
            trayIcon.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Occurs when the user ends the Windows session by logging off or shutting down
        /// the operating system.
        /// </summary>
        /// <param name="e">A System.Windows.SessionEndingCancelEventArgs that contains the event data.</param>
        protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
        {
            trayIcon.Visibility = Visibility.Collapsed;
        }

        private void OnKeyPressed(object sender, KeyPressedEventArgs e)
        {
            // Show the search window.
            m_window?.Show();
        }

        private void OnSettingsItemClick(object sender, RoutedEventArgs e)
        {
            s_window ??= new SettingsWindow();
            s_window.Show();
        }

        private void OnExitItemClick(object sender, RoutedEventArgs e)
        {
            Shutdown();
        }

        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [STAThreadAttribute()]
        [DebuggerNonUserCodeAttribute()]
        public static void Main()
        {
            Process currentProcess = Process.GetCurrentProcess();
            Process runningProcess = (from process in Process.GetProcesses()
                                       where
                                         process.Id != currentProcess.Id &&
                                         process.ProcessName.Equals(
                                           currentProcess.ProcessName,
                                           StringComparison.Ordinal)
                                       select process).FirstOrDefault();
            if (runningProcess != null)
            {
                currentProcess.Kill();
                return;
            }
            App app = new();
            app.Run();
        }

        private Window m_window;
        private Window s_window;
        private KeyboardHook keyHook;
        private TaskbarIcon trayIcon;
    }
}
