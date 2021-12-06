using System.Windows.Threading;
using System.Windows.Input;
using System.Windows;
using System;
using System.Windows.Controls;
using Windows.Management.Deployment;
using Windows.Foundation;
using Windows.ApplicationModel;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace QuickSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            IsVisibleChanged += OnVisibilityChanged;
        }

        protected override async void OnActivated(EventArgs e)
        {
            await Dispatcher.BeginInvoke(DispatcherPriority.Input, () =>
            {
                Keyboard.Focus(searchBox);
            });
        }

        protected override void OnDeactivated(EventArgs e)
        {
            Hide();
        }

        private void OnVisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool visible && visible)
                Activate();
        }

        private void OnSearchBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (tokenSource != null)
                tokenSource.Cancel();
        }

        private async void OnSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchBox.Text != "")
            {
                resultsGrid.Visibility = Visibility.Visible;

                tokenSource = new();
                token = tokenSource.Token;

                appsListBox.ItemsSource = await FilterApplicationsAsync(searchBox.Text, token);
            }
            else
            {
                resultsGrid.Visibility = Visibility.Collapsed;
            }
        }

        private IAsyncOperation<IEnumerable<Package>> FilterApplicationsAsync(string key, CancellationToken token)
        {
            return Task.Run( () =>
            {
                List<Package> packages = new();

                foreach (Package package in packageManager.FindPackagesForUser(""))
                {
                    if (!token.IsCancellationRequested)
                    {
                        if (package.DisplayName.ToLower().Contains(key.ToLower()))
                            packages.Add(package);
                    }
                    else
                    {
                        return null;
                    }
                }
                return packages.AsEnumerable();
            }, token).AsAsyncOperation();
        }

        private CancellationToken token;
        private CancellationTokenSource tokenSource;
        private readonly PackageManager packageManager = new();
        private readonly string packagesKey = @"SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\Repository\Packages";
        private readonly string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
    }
}
