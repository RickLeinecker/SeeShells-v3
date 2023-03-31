using System;

namespace SeeShellsV3.Data
{
    public class Timezone : ITimezone
    {
        public string Name
        {
            get
            {
                bool isInDaylight = Information.SupportsDaylightSavingTime && Information.IsDaylightSavingTime(DateTime.Now);
                return isInDaylight ? DaylightName : DisplayName;
            }
        }
        /// <summary>
        /// The timezone name in daylight savings.
        /// </summary>
        private string DaylightName { get; init; }

        /// <summary>
        /// The display name of the timezone if not in daylight savings.
        /// </summary>
        private string DisplayName { get; init; }

        public string Registry { get; init; }
        public string Offset { get; init; }
        public TimeZoneInfo Information { get; init; }

        /// <summary>
        /// Constructs a timezone object
        /// </summary>
        /// <param name="registryName">The name of the timezone as it appears in the registry.</param>
        /// <param name="displayName">Optional parameter to display the name differently than the registry name.</param>
        /// <param name="offset">The offset of the timezone in the format "+/-XX:XX"</param>
        public Timezone(string registryName, string displayName = null, string offset = "")
        {
            Registry = registryName;
            Information = TimeZoneInfo.FindSystemTimeZoneById(registryName);

            DisplayName = displayName ?? registryName;
            DaylightName = Information.SupportsDaylightSavingTime ? Information.DaylightName : null;

            Offset = $"(UTC {offset})";
        }

        public bool Identify(string input)
        {
            return input == Registry || input == Name;
        }

        public bool Identify(TimeZoneInfo input)
        {
            return input.Equals(Information);
        }

        public int CompareTo(ITimezone other)
        {
            return (Offset, Name).CompareTo((other.Offset, other.Name));
        }
    }
}