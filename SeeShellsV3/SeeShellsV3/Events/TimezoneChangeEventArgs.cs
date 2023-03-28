using System;

namespace SeeShellsV3.Events
{
    public class TimezoneChangeEventArgs : EventArgs
    {
        public TimezoneChangeEventArgs(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name of the new timezone in standard format.
        /// </summary>
        /// <example>
        /// Eastern time would be formatted as "Eastern Standard Time".
        /// </example>
        public string Name { get; init; }
    }
}