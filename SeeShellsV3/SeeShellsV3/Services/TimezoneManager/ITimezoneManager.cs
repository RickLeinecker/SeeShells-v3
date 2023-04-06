using System;
using System.Collections.ObjectModel;
using System.IO;
using SeeShellsV3.Data;
using SeeShellsV3.Events;
using SeeShellsV3.Repositories;

namespace SeeShellsV3.Services
{
    /// <summary>
    /// An object that manages the changing of the timezones within the application.
    /// </summary>
    public interface ITimezoneManager
    {
        /// <summary>
        /// The currently selected timezone.
        /// </summary>
        public Timezone CurrentTimezone { get; set; }

        /// <summary>
        /// A list of timezones that are currently supported.
        /// </summary>
        public ITimezoneCollection SupportedTimezones { get; set; }

        public Timezone GetTimezone(string input);

        /// <summary>
        ///  Handles the changing of timestamps throughout the application.
        /// </summary>
        /// <param name="timezone">Name of the timezone that will be changed to.</param>
        public void TimezoneChangeHandler(string timezone, bool reload = false);

        /// <summary>
        /// Handles the changing of timestamps throughout the application.
        /// </summary>
        /// <param name="timezone">Timezone object representing the timezone that will be changed to.</param>
        public void TimezoneChangeHandler(Timezone timezone, bool reload = false);

        /// <summary>
        /// Refreshes all timestamps to the current timezone. Used in situations where more data is pulled in after
        /// the timezone was switched.
        /// </summary>
        public void ReloadTimezones();
    }
}