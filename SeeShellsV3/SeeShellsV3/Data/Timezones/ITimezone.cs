using System;

namespace SeeShellsV3.Data
{
    public interface ITimezone: IComparable<ITimezone>
    {
        /// <summary>
        /// The display name of the timezone. Reflects daylight savings. Access this for any display.
        /// </summary>
        /// <example>
        /// Eastern time would be formatted as "Eastern Standard Time".
        /// </example>
        string Name { get; }

        /// <summary>
        /// The name of the timezone as it is in the registry. Access this for any
        /// registry related searches. Used since daylight times do not
        /// change names in the registry.
        /// </summary>
        /// <example>
        /// Eastern time during daylight savings in named "Eastern Daylight Time", however,
        /// it remains as "Eastern Standard Time" in the registry.
        /// </example>
        string Registry { get; init; }

        /// <summary>
        /// The offset of the timezone in the format (UTC +/-XX:XX)
        /// </summary>
        string Offset { get; init; }

        /// <summary>
        /// A TimeZoneInfo object that represents the timezone.
        /// </summary>
        TimeZoneInfo Information { get; init; }

        bool Identify(string input);
        bool Identify(TimeZoneInfo input);
    }
}
