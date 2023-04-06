using ControlzEx.Standard;
using System;

namespace SeeShellsV3.Data
{
    public class Timezone : ITimezone
    {
        public string Name => isDaylightSavings ? DaylightName : DisplayName;
        public string Header => $"{Offset} {Name}";

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
        public string Locale { get; init; }
        public string DaylightStatus { get; init; }
        private bool isDaylightSavings { get; init; }
        private bool hasDaylightSavings { get; init; }
        public TimeZoneInfo Information { get; init; }

        /// <summary>
        /// Constructs a timezone object
        /// </summary>
        /// <param name="registryName">The name of the timezone as it appears in the registry.</param>
        /// <param name="displayName">Optional parameter to display the name differently than the registry name.</param>
        /// <param name="offset">The offset of the timezone in the format "+/-XX:XX"</param>
        public Timezone(string registryName, string displayName = null)
        {
            Registry = registryName;
            Information = TimeZoneInfo.FindSystemTimeZoneById(registryName);

            DisplayName = displayName ?? registryName;
            DaylightName = Information.SupportsDaylightSavingTime ? Information.DaylightName : null;

            hasDaylightSavings = Information.SupportsDaylightSavingTime;
            isDaylightSavings =  Information.IsDaylightSavingTime(DateTime.Now);
            if (hasDaylightSavings)
                DaylightStatus = isDaylightSavings ? "Yes" : "No";
            else
                DaylightStatus = "N/A";

            Offset = CalculateOffset();

            Locale = CalculateLocale();
        }

        private string CalculateOffset()
        {
            string info = Information.DisplayName;

            int start = info.IndexOf("(", StringComparison.Ordinal);
            int end = info.IndexOf(")", StringComparison.Ordinal) + 1;

            return info.Substring(start, end-start);
        }

        private string CalculateLocale()
        {
            string info = Information.DisplayName;

            return info.Substring(info.IndexOf(")", StringComparison.Ordinal) + 2);
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