﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

using Unity;
using Newtonsoft.Json;

using SeeShellsV3.Data;
using SeeShellsV3.Factories;
using SeeShellsV3.Repositories;
using SeeShellsV3.Services;
using SeeShellsV3.UI;

namespace SeeShellsV3
{
    /// <summary>
    /// SeeShellsV3 main entry point
    /// </summary>
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            // Construct the root container for this application. This container will be used
            // to resolve the dependencies of all object instances throughout the lifetime of the application.
            IUnityContainer container = new UnityContainer();

            // Read Config.JSON to get registry importer settings as an IConfig object.
            Assembly assembly = Assembly.GetExecutingAssembly();
            string internalResourcePath = assembly.GetManifestResourceNames().Single(str => str.EndsWith("Config.json"));
            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(internalResourcePath)))
                container.RegisterInstance<IConfig>(JsonConvert.DeserializeObject<Config>(reader.ReadToEnd()));

            // Register Factory Types
            container.RegisterType<IWindowFactory, WindowFactory>();
            container.RegisterType<IShellItemFactory, ShellItemFactory>();
            container.RegisterType<IShellEventFactory, ShellEventFactory>();

            // Register Repository Types
            container.RegisterSingleton<IUserCollection, UserCollection>();
            container.RegisterSingleton<IRegistryHiveCollection, RegistryHiveCollection>();
            container.RegisterSingleton<IShellItemCollection, ShellItemCollection>();
            container.RegisterSingleton<IShellEventCollection, ShellEventCollection>();
            container.RegisterSingleton<ITimezoneCollection, TimezoneCollection>();
            container.RegisterSingleton<ISelected, Selected>();
            container.RegisterSingleton<IReportEventCollection, ReportEventCollection>();

            // Register Service Types
            container.RegisterType<IPdfExporter, PdfExporter>();
            container.RegisterType<IRegistryImporter, RegistryImporter>();
            container.RegisterType<IShellEventManager, ShellEventManager>();
            container.RegisterSingleton<ITimezoneManager, TimezoneManager>();
            container.RegisterSingleton<IPaletteManager, PaletteManager>();

            // Register Window Types
            container.RegisterType<IWindow, MainWindow>("main");
            container.RegisterType<IWindow, ExportWindow>("export");
            container.RegisterType<IWindow, TimezoneWindow>("timezones");

            // Register ViewModel Types
            container.RegisterType<IMainWindowVM, MainWindowVM>();
            container.RegisterType<IExportWindowVM, ExportWindowVM>();
            container.RegisterType<ITimezoneWindowVM, TimezoneWindowVM>();
            container.RegisterType<IInspectorViewVM, InspectorViewVM>();
            container.RegisterType<ITimelineViewVM, TimelineViewVM>();
            container.RegisterType<IRegistryViewVM, RegistryViewVM>();
            container.RegisterType<IFilterControlViewVM, FilterControlViewVM>();
            container.RegisterType<IHexViewVM, HexViewVM>();

            // Create and run app with main window. The main window is contructed in SeeShellsV3.UI.App.OnStartup().
            App app = container.Resolve<App>();
            app.Run();
        }
    }
}
