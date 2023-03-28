using System;
using System.Collections.ObjectModel;
using SeeShellsV3.Data;
using SeeShellsV3.Events;

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
        public Collection<Timezone> SupportedTimezones { get; init; }

        public Timezone GetTimezone(string input);

        public void TimezoneChangeHandler(string timezone);
    }
}