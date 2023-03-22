using System;

namespace SeeShellsV3.Data
{
    public interface ITimezone
    {
        /// <summary>
        /// The name of the timezone in standard format.
        /// </summary>
        /// <example>
        /// Eastern time would be formatted as "Eastern Standard Time".
        /// </example>
        string Name { get; init; }

        /// <summary>
        /// The name of the new timezone in standard format.
        /// </summary>
        /// <example>
        /// Eastern time would be formatted as "Eastern Standard Time".
        /// </example>
        string Identifier { get; init; }

        /// <summary>
        /// A TimeZoneInfo object that represents the timezone.
        /// </summary>
        TimeZoneInfo Information { get; init; }
    }
}
