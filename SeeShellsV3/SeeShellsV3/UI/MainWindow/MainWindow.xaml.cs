﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Win32;
using ControlzEx.Theming;

using MahApps.Metro.Controls;
using SeeShellsV3.Events;
using Unity;

using SeeShellsV3.Factories;
using System.Collections.Generic;
using System.Windows.Media;

namespace SeeShellsV3.UI
{
    public interface IMainWindowVM : IViewModel
    {
        bool ImportFromRegistry(string hiveLocation = null);
        void RestartApplication(bool runAsAdmin = false);
        void ChangeTimezone(string timezone);
        string WebsiteUrl { get; }
        string GithubUrl { get; }

        Visibility StatusVisibility { get; }
        string Status { get; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, IWindow
    {
        [Dependency]
        public IMainWindowVM ViewModel
        {
            private get { return DataContext as IMainWindowVM; }
            set { DataContext = value; }
        }

        [Dependency]
        public IWindowFactory WindowFactory { private get; set; }

        Uri currTheme;

        public MainWindow()
        {
            currTheme = new Uri(@"UI/Themes/DarkTheme.xaml", UriKind.Relative);
            InitializeComponent();
        }

        private void Export_Window_Click(object sender, RoutedEventArgs e)
        {
            IWindow win = WindowFactory.Create("export");
            win.Show();
        }

		private void Import_Live_Registry_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ImportFromRegistry();
        }

        private void Import_Offline_Registry_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { ValidateNames = false, ReadOnlyChecked = true };
            if (openFileDialog.ShowDialog() == true)
                ViewModel.ImportFromRegistry(openFileDialog.FileName);
        }

        private void ResetMenuItem_Click(object sender, RoutedEventArgs e)
        {
            bool isElevated;
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            MessageBoxResult result = MessageBox.Show(
                    "Are you sure you want to reset the application (progress will be lost)?",
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                ViewModel.RestartApplication(isElevated);
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch s && s.IsOn)
                currTheme = new Uri(@"UI/Themes/DarkTheme.xaml", UriKind.Relative);
            else
                currTheme = new Uri(@"UI/Themes/LightTheme.xaml", UriKind.Relative);
            (Application.Current as App).ChangeTheme(currTheme);
        }

        private void SplitButton_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (sender is SplitButton splitButton)
            {
                TimeSeriesHistogram histogram = FindName("Histogram") as TimeSeriesHistogram;
                if (histogram != null) {
                    histogram.histPlotModel_setColors(splitButton.SelectedIndex);
                    System.Diagnostics.Debug.WriteLine("Histogram is not null");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Histogram is null");
                }
            }
        }

        private void ChangeTimezone_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ChangeTimezone((sender as MenuItem).Header as string);
        }
    }
}
