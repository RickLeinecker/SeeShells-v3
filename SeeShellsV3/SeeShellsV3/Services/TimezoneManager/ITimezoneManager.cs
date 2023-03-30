﻿using System;
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

        /// <summary>
        ///  Handles the changing of timestamps throughout the application.
        /// </summary>
        /// <param name="timezone">Name of the timezone that will be changed to.</param>
        public void TimezoneChangeHandler(string timezone);

        /// <summary>
        /// Handles the changing of timestamps throughout the application.
        /// </summary>
        /// <param name="timezone">Timezone object representing the timezone that will be changed to.</param>
        public void TimezoneChangeHandler(Timezone timezone);
    }
}