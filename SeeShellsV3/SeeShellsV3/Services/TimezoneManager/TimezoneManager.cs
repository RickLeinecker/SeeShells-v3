using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using CsvHelper;
using SeeShellsV3.Data;
using SeeShellsV3.Events;
using SeeShellsV3.Repositories;
using SeeShellsV3.UI;
using Unity;

namespace SeeShellsV3.Services
{
    public class TimezoneManager: ITimezoneManager
    {
        public Timezone CurrentTimezone { get; set; } = new Timezone("UTC", displayName: "Coordinated Universal Time");

         public ITimezoneCollection SupportedTimezones { get; set; }
        private IShellEventCollection ShellEvents { get; set; }
        private IShellItemCollection ShellItems { get; set; }
        private ISelected Selected { get; set; }

        [Dependency] public IShellEventManager ShellEventManager { get; set; }



        public TimezoneManager(
            [Dependency] IShellEventCollection shellEvents,
            [Dependency] IShellItemCollection shellItems,
            [Dependency] ISelected selected,
            [Dependency] ITimezoneCollection timezones
        )
        {
            ShellItems = shellItems;
            ShellEvents = shellEvents;
            Selected = selected;
            SupportedTimezones = timezones;

            LoadSupportedTimezones();
        }

        public void TimezoneChangeHandler(Timezone timezone)
        {
            // Store the timezone that we are switching from for conversion purposes
            Timezone oldTimezone = CurrentTimezone;

            
            // Update CurrentTimezone to the new timezone
            CurrentTimezone = timezone;

            if (oldTimezone.Equals(CurrentTimezone) || oldTimezone.Registry == CurrentTimezone.Registry)
            {
                return;
            }

            // Loop through all ShellItems and update their timestamps
            foreach (var shellItem in ShellItems)
            {
                // Update timestamps that all ShellItems have
                shellItem.SlotModifiedDate = ConvertTimezone(shellItem.SlotModifiedDate, oldTimezone);
                shellItem.LastRegistryWriteDate = ConvertTimezone(shellItem.LastRegistryWriteDate, oldTimezone);

                // Switch on ShellItem type to update timestamps that only specific types of ShellItems have
                switch (shellItem)
                {
                    case CompressedFolderShellItem compressedFolderShellItem:
                        compressedFolderShellItem.ModifiedDate = ConvertTimezone(compressedFolderShellItem.ModifiedDate, oldTimezone);
                        break;
                    case FileEntryShellItem fileEntryShellItem:
                        fileEntryShellItem.ModifiedDate = ConvertTimezone(fileEntryShellItem.ModifiedDate, oldTimezone);
                        fileEntryShellItem.AccessedDate = ConvertTimezone(fileEntryShellItem.AccessedDate, oldTimezone);
                        fileEntryShellItem.CreationDate = ConvertTimezone(fileEntryShellItem.CreationDate, oldTimezone);
                        break;
                    case UriShellItem uriShellItem:
                        uriShellItem.ConnectedDate = ConvertTimezone(uriShellItem.ConnectedDate, oldTimezone);
                        break;
                }
            }


            // Loop through all ShellEvents and update their timestamps
            foreach (ShellEvent shellEvent in ShellEvents)
            {
                shellEvent.TimeStamp = ConvertTimezone(shellEvent.TimeStamp, oldTimezone);
            }

            // Update ShellEvent collection so that the timeline gets updated
            CollectionViewSource viewSource = new CollectionViewSource();
            if (Selected.CurrentInspector is ShellEvent temp) // Ensure Selected.CurrentInspector is correct type
            {
                foreach (ShellItem i in temp.Evidence)
                {
                    viewSource.Source = i.ActualFields;
                    ICollectionView view = viewSource.View;

                    view.Refresh();
                }
            }

            Selected.CurrentInspector = null;
            Selected.CurrentData = null;

            ShellEvents.FilteredView.Refresh();
            ShellItems.FilteredView.Refresh();
        }

        public void TimezoneChangeHandler(string timezone)
        {
            TimezoneChangeHandler(GetTimezone(timezone));
        }

        /// <summary>
        /// Wrapper for the various ConvertTime functions to avoid errors when converting DateKind objects
        /// with type Utc. Also allows nullable DateTimes to be converted.
        /// </summary>
        /// <param name="dateTime">DateTime object to be converted.</param>
        /// <param name="oldTimezone">The timezone being switched from.</param>
        /// <returns>A DateTime object representing the same time <see cref="input"/> does, in the timezone of <see cref="CurrentTimezone"/></returns>
        private DateTime ConvertTimezone(DateTime dateTime, Timezone oldTimezone)
        {
            if (CurrentTimezone.Registry == "UTC")
            {
                return TimeZoneInfo.ConvertTimeToUtc(dateTime, oldTimezone.Information);
            }
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(dateTime, CurrentTimezone.Information);
            }
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                return TimeZoneInfo.ConvertTime(dateTime, oldTimezone.Information, CurrentTimezone.Information);
            }
            if (dateTime.Kind == DateTimeKind.Local)
            {
                return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Local, CurrentTimezone.Information);
            }

            throw new TimezoneNotSupportedException();
        }

        private DateTime? ConvertTimezone(DateTime? input, Timezone oldTimezone)
        {
            if (input is null)
            {
                return null;
            }

            DateTime dateTime = Convert.ToDateTime(input);

            return ConvertTimezone(dateTime, oldTimezone);
        }

        public Timezone GetTimezone(string input)
        {
            foreach (var timezone in SupportedTimezones)
            {
                if (timezone.Identify(input))
                    return timezone as Timezone;
            }

            throw new TimezoneNotSupportedException();
        }

        /// <summary>
        /// Populates a collection of supported timezones which are loaded from Timezones.csv
        /// </summary>
        private void LoadSupportedTimezones()
        {
            // Load the CSV file
            Assembly assembly = Assembly.GetExecutingAssembly();
            string internalResourcePath = assembly.GetManifestResourceNames().Single(str => str.EndsWith("Timezones.csv"));
            StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(internalResourcePath));

            // Create and setup a CSV Reader
            CsvReader csv = new CsvReader(reader, CultureInfo.CurrentCulture);
            csv.Read();
            csv.ReadHeader();
            csv.Context.TypeConverterOptionsCache.GetOptions<string>().NullValues.Add("null");

            // Loop through the CSV file, reading each entry and creating a timezone object from the information
            while (csv.Read())
            {
                string registry = csv.GetField<string>("RegistryName");
                string display = csv.GetField<string>("DisplayName");

                Timezone current = display != null ? new Timezone(registry, displayName: display) : new Timezone(registry);
                SupportedTimezones.Add(current);
            }
        }
    }

    public class TimezoneNotSupportedException : Exception
    {
        public TimezoneNotSupportedException() {}
        public TimezoneNotSupportedException(string message) : base(message) { }

        public TimezoneNotSupportedException(string message, Exception innerException) : base(message, innerException) {}
    }
}