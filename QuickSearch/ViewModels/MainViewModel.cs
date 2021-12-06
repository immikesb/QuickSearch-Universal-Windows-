using Microsoft.Win32;
using QuickSearch.ViewModels.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Management.Deployment;

namespace QuickSearch.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            
        }

        #region Properties

        public string Text
        {
            get => text;
            set => SetValue(ref text, value);
        }

        public ObservableCollection<Package> ApplicationResultList
        {
            get => applicationResultList;
        }

        #endregion

        #region Methods

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
            {
                applicationResultList.Clear();
                if (Text == "")
                {
                    return;
                }
                applicationResultList.Add((from Package package in packageManager.FindPackagesForUser("")
                                         where
                                            package.DisplayName.ToLower().Contains(Text.ToLower())
                                         select package).FirstOrDefault());
#if DEBUG
                System.Diagnostics.Debug.WriteLine(ApplicationResultList.FirstOrDefault()?.DisplayName);
#endif
            }
#if DEBUG
            System.Diagnostics.Debug.WriteLine(e.PropertyName + " has changed");
#endif
        }

        /*
        private async Task FilterApplications(string appName)
        {
            await Task.Run(() =>
            {
                using var rk = Registry.LocalMachine.OpenSubKey(uninstallKey);
                foreach (var skName in rk?.GetSubKeyNames())
                {
                    using var sk = rk?.OpenSubKey(skName);
                    try
                    {
                        //var path = sk.GetValue("InstallSource") ?? sk.GetValue("InstallLocation") ?? "";
                        if (sk?.GetValue("DisplayName") is string displayName && displayName.ToLower().Contains(Text.ToLower()))
                            System.Diagnostics.Debug.WriteLine(displayName);
                    }
                    catch { }
                    sk?.Dispose();
                }
                rk?.Dispose();
            });
        }
        */

        private IAsyncAction FilterApplicationsAsync(string key)
        {
            return Task.Run(() =>
            {
                IEnumerable<Package> packages = from Package package in packageManager.FindPackagesForUser("")
                                                 where
                                                    package.DisplayName.ToLower().Contains(Text.ToLower())
                                                 select package;
                if (packages is not null)
                {

                }
            }).AsAsyncAction();
        }

        #endregion

        private string text = "";
        private readonly PackageManager packageManager = new();
        private ObservableCollection<Package> applicationResultList = new();
        private const string packagesKey = @"SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\Repository\Packages";
        private const string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
    }
}
