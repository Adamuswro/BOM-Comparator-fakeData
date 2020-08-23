using BOMComparator.Core.DataAccessDB;
using BOMComparator.Core.Models;
using BOMComparator.ViewModels.Helpers;
using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace BOMComparator.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        private IMotorService _motorService;
        private bool _isAuthorized = false;
        public bool IsAuthorized 
        { 
            get => _isAuthorized;
            private set
            {
                _isAuthorized = value;
                NotifyOfPropertyChange (() => CanLoadFiles);
                NotifyOfPropertyChange (() => CanClearCache);
                NotifyOfPropertyChange(() => CanShowSearchView);
                NotifyOfPropertyChange(() => CanShowSettings);
            } 
        }
        public bool CanLoadFiles { get => IsAuthorized; }
        public bool CanClearCache { get => IsAuthorized; }
        public bool CanShowSearchView { get => IsAuthorized; }
        public bool CanShowSettings { get => IsAuthorized; }
        public static BindableCollection<LogEntry> Logs { get; protected set; } = new BindableCollection<LogEntry>();

        public ShellViewModel(IMotorService motorService)
        {
            var logingScreen = new LogingViewModel();
            logingScreen.LoginSucceedEventHandler += LoggedIn;
            ActivateItem(logingScreen);

            _motorService = motorService;
        }

        public static void Log(string message)
        {
            Logs.Insert(0, new LogEntry(message));
        }

        public void ClearCache()
        {
            try
            {
                _motorService.ClearLoadedData();
            }
            catch (Exception ex)
            {
                Log($"Cache clear failed! Error:{Environment.NewLine}{ex.Message}");
            }
            Log("Cache clear succeed.");
        }

        public void LoadFiles()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = "Select excel files with motors BOM (CEWB or export from TC 7.5)",
                Filter = "Excel |*.xlsx",
                FilterIndex = 1,
                InitialDirectory = FilePathHelper.GetExternalFilesPath(),
                Multiselect = true,
            };

            var motorsCount = _motorService.AllMotors.Count();
            var partsCount = _motorService.AllParts.Count();

            if (openFileDialog.ShowDialog() == true)
            {
                var watchDatabaseLoading = System.Diagnostics.Stopwatch.StartNew();
                using (new WaitCursor())
                {
                    try
                    {
                        _motorService.LoadFilesAndUpdateDatabase(openFileDialog.FileNames);
                    }
                    catch (Exception ex)
                    {
                        Log($"Operation failed. Error: {ex.Message}");
                        return;
                    }
                }
                watchDatabaseLoading.Stop();
                var fileNames = String.Join("; ", openFileDialog.FileNames.Select(p => Path.GetFileName(p)));
                ShellViewModel.Log($"Loading files {fileNames} succeed.");
            }
        }
        public void LoggedIn()
        {
            IsAuthorized = true;
            ShowSearchView("Welcome!");
        }
        public void ShowSearchView(string communicate = "Home view selected.")
        {
            if (!String.IsNullOrEmpty(communicate))
            {
                Log(communicate);
            }
            ActivateItem(new SearchManagerViewModel(IoC.Get<IMotorService>()));
        }

        public void ShowSettings()
        {
            Log("Settings selected.");
            ActivateItem(new SettingsViewModel());
        }

        public void About(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Version 1.0 alfa." + "\n" + "Created by Adam Szulc.");
        }
    }

}
