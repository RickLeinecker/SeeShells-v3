using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CsvHelper;
using Unity;

using SeeShellsV3.Data;
using SeeShellsV3.Events;
using SeeShellsV3.Repositories;
using SeeShellsV3.Services;

namespace SeeShellsV3.UI
{
    public class MainWindowVM : ViewModel, IMainWindowVM
    {
        [Dependency] public IRegistryImporter RegImporter { get; set; }
        [Dependency] public IShellEventManager ShellEventManager { get; set; }
        [Dependency] public IShellEventCollection ShellEvents { get; set; }
        [Dependency] public ITimezoneManager TimezoneManager { get; set; }
        [Dependency] public ISelected Selected { get; set; }
        [Dependency] public IReportEventCollection ReportEvents { get; set; }

        public string WebsiteUrl => @"https://rickleinecker.github.io/SeeShells-V3";
        public string GithubUrl => @"https://github.com/RickLeinecker/SeeShells-V3";

        public Visibility StatusVisibility => Status != string.Empty ? Visibility.Visible : Visibility.Collapsed;
        public string Status { get => _status; private set { _status = value; NotifyPropertyChanged(nameof(Status)); NotifyPropertyChanged(nameof(StatusVisibility)); } }
        private string _status = string.Empty;

        public void RestartApplication(bool runAsAdmin = false)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
            proc.StartInfo.UseShellExecute = true;

            if (runAsAdmin)
                proc.StartInfo.Verb = "runas";

            proc.Start();
            Application.Current.Shutdown();
        }

        public bool ImportFromRegistry(string hiveLocation = null)
        {
            bool isElevated;
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            if (isElevated || hiveLocation != null)
            {
                ImportFromRegistryInternal(hiveLocation);
                return true;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                    "Active Registry Import requires administrator access. Do you want to restart the application as administrator (this will reset the application)?",
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                    RestartApplication(true);
            }

            return false;
        }

        private async void ImportFromRegistryInternal(string hiveLocation = null)
        {
            Status = "Parsing Registry Hive...";
            (RegistryHive root, IEnumerable<IShellItem> parsedItems) = hiveLocation == null ?
                await Task.Run(() => RegImporter.ImportRegistry(true)) :
                await Task.Run(() => RegImporter.ImportRegistry(false, true, hiveLocation));

            if (root == null || parsedItems == null)
            {
                Status = "No New Shellbags Found.";
            }
            else
            {
                Selected.CurrentInspector = root;

                Status = "Generating User Action Events...";
                await Task.Run(() => ShellEventManager.GenerateEvents(parsedItems));
                Status = "Done.";
            }

            await Task.Run(() => Thread.Sleep(3000));
            Status = string.Empty;
        }

        public void ExportToCSV(string filePath, string source)
        {
            StreamWriter writer = new StreamWriter(filePath);
            CsvWriter csv = new CsvWriter(writer, CultureInfo.CurrentCulture);

            // Determine whether the events added to the report should be exported or the filtered view
            // should be exported based on user input
            ICollectionView eventSource = ShellEvents.FilteredView;
            if (source == "Export Selected")
            {
                eventSource = ReportEvents.SelectedEvents.FilteredView;
            }

            foreach (ShellEvent shellEvent in eventSource)
            {
                csv.WriteField(shellEvent.TimeStamp);
                csv.WriteField(shellEvent.Description);
                csv.WriteField(shellEvent.TypeName);
                csv.WriteField(shellEvent.User.Name);
                csv.WriteField(shellEvent.Place.Name);
                csv.WriteField(shellEvent.Place.PathName);

                csv.NextRecord();
            }

            csv.Flush();
            writer.Close();
        }

        // TODO: Handle errors
        public void AddToReportCollection()
        {
            IShellEvent shell = Selected.CurrentInspector as IShellEvent;
            ReportEvents.Add(shell);
        }
        public void ChangeTimezone(string timezone)
        {
            Debug.WriteLine("ChangeTime");
            TimezoneManager.TimezoneChangeHandler(timezone);
            NotifyPropertyChanged(nameof(TimezoneManager));
        }
    }
}
